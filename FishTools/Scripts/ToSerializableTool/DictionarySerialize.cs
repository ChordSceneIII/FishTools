using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tvalue和Tkey如果是不支持序列化的类型则会无视
/// 如果要使用持久化存储，请务必保证key和value是支持序列化的类型
/// </summary>


namespace FishTools
{
    [Serializable]
    public struct SerializePair<Tkey, Tvalue>
    {

        public Tkey key;
        public Tvalue value;

        public SerializePair(Tkey key, Tvalue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public class DictionarySerializable<Tkey, TValue> : ISerializationCallbackReceiver
    {
        // 序列化列表 (只用于显示和序列化)
        [SerializeField]
        private List<SerializePair<Tkey, TValue>> _pairs = new List<SerializePair<Tkey, TValue>>();

        // 实际字典 (用于所有增删改查操作)
        private Dictionary<Tkey, TValue> _dict = new Dictionary<Tkey, TValue>();

        // 新增一个标志来检测字典是否发生了变化
        private bool _isDirty = false;

        public void OnBeforeSerialize()
        {

            // 检查键的唯一性
            HashSet<Tkey> keySet = new HashSet<Tkey>();
            bool hasDuplicates = false;

            foreach (var pair in _pairs)
            {
                if (!keySet.Add(pair.key))
                {
                    // 发现重复的键，提示用户
#if UNITY_EDITOR
                    Debug.LogError($"Duplicate key detected during serialization: {pair.key}");
#endif
                    hasDuplicates = true;
                }
            }

            // 如果有重复键，则不进行同步
            if (!hasDuplicates)
            {
                if (_isDirty)
                {
                    SyncSerializableData();
                    _isDirty = false;
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Cannot serialize because duplicate keys were found.");
#endif
            }
        }

        public void OnAfterDeserialize()
        {
            SyncDictionaryData();
        }

        private void SyncSerializableData()
        {
            _pairs.Clear();
            foreach (var kvp in _dict)
            {
                _pairs.Add(new SerializePair<Tkey, TValue>(kvp.Key, kvp.Value));
            }
        }

        private void SyncDictionaryData()
        {
            _dict.Clear();
            foreach (var pair in _pairs)
            {
                _dict[pair.key] = pair.value;
            }
        }

        // 修改增删改查操作，更新 _isDirty 标志
        public void Add(Tkey key, TValue value)
        {
            if (!_dict.ContainsKey(key))
            {
                _dict.Add(key, value);
                _pairs.Add(new SerializePair<Tkey, TValue>(key, value));
                _isDirty = true;  // 标记字典已更改
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Key already exists in the dictionary.");
#endif
            }
        }

        public bool Remove(Tkey key)
        {
            if (_dict.Remove(key))
            {
                _pairs.RemoveAll(pair => EqualityComparer<Tkey>.Default.Equals(pair.key, key));
                _isDirty = true;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _dict.Clear();
            _pairs.Clear(); // 同时清空序列化字典
        }

        public TValue this[Tkey key]
        {
            get => _dict[key];
            set
            {
                if (_dict.ContainsKey(key))
                {
                    _dict[key] = value;
                    UpdateSerializedPair(key, value);
                    _isDirty = true;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        private void UpdateSerializedPair(Tkey key, TValue value)
        {
            // 查找键是否已经存在于 _pairs 列表中
            for (int i = 0; i < _pairs.Count; i++)
            {
                if (EqualityComparer<Tkey>.Default.Equals(_pairs[i].key, key))
                {
                    _pairs[i] = new SerializePair<Tkey, TValue>(key, value);
                    _isDirty = true;
                    return;
                }
            }

            // 如果键不存在，添加新键值对
            _pairs.Add(new SerializePair<Tkey, TValue>(key, value));
        }

        public bool TryGetValue(Tkey key, out TValue value) => _dict.TryGetValue(key, out value);

        public bool ContainsKey(Tkey key) => _dict.ContainsKey(key);


        // 获取字典的 Keys
        public Dictionary<Tkey, TValue>.KeyCollection Keys => _dict.Keys;

        // 获取字典的 Values
        public Dictionary<Tkey, TValue>.ValueCollection Values => _dict.Values;


        // [静态方法] 从 Dictionary 转换到 DictionarySerializable
        public static DictionarySerializable<Tkey, TValue> ToSerializable(Dictionary<Tkey, TValue> dict)
        {
            var serializableDict = new DictionarySerializable<Tkey, TValue>();

            foreach (var kvp in dict)
            {
                serializableDict.Add(kvp.Key, kvp.Value);
            }

            return serializableDict;
        }
    }
}
