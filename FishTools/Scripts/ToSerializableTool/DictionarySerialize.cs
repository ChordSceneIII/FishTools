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
                    DebugF.LogError($"key重复{pair.key}");
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
                DebugF.LogError("无法同步到实际字典，序列化列表中中存在重复的键");
            }
        }

        public void OnAfterDeserialize()
        {
            SyncDictionaryData();
        }

        // 同步序列化列表
        private void SyncSerializableData()
        {
            _pairs.Clear();
            foreach (var kvp in _dict)
            {
                _pairs.Add(new SerializePair<Tkey, TValue>(kvp.Key, kvp.Value));
            }
        }

        // 同步实际字典
        private void SyncDictionaryData()
        {
            _dict.Clear();
            foreach (var pair in _pairs)
            {
                _dict[pair.key] = pair.value;
            }
        }

        // 增加
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
                DebugF.LogError("Key已经存在Dicitonary中");
            }
        }

        // 移除
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

        // 插入
        public void InsertAt(int index, Tkey key, TValue value)
        {
            if (_dict.ContainsKey(key))
            {
                DebugF.LogError("Key已经存在Dicitonary中,无法插入。");
                return; // 如果键已存在，则不插入
            }

            // 确保索引在有效范围内
            if (index < 0 || index > _pairs.Count)
            {
                DebugF.LogError("插入位置超出范围。");
                return; // 索引超出范围
            }

            // 插入到 _pairs 列表
            _pairs.Insert(index, new SerializePair<Tkey, TValue>(key, value));

            // 更新字典
            _dict[key] = value; // 添加到字典

            _isDirty = true; // 标记字典已更改
        }

        //修改Key值
        public bool UpdateKey(Tkey oldKey, Tkey newKey)
        {
            if (_dict.TryGetValue(oldKey, out TValue value))
            {
                // 检查新键是否已存在
                if (_dict.ContainsKey(newKey))
                {
                    DebugF.LogError($"新键 {newKey} 已存在，无法修改。");
                    return false; // 新键已存在，无法更新
                }

                // 移除旧的键值对
                _dict.Remove(oldKey);

                // 更新 _pairs 中的键
                for (int i = 0; i < _pairs.Count; i++)
                {
                    if (EqualityComparer<Tkey>.Default.Equals(_pairs[i].key, oldKey))
                    {
                        // 更新键
                        _pairs[i] = new SerializePair<Tkey, TValue>(newKey, value);
                        break;
                    }
                }
                // 添加新的键值对
                _dict.Add(newKey, value);
                _isDirty = true;  // 标记字典已更改
                return true;
            }

            // 返回 false 表示旧键不存在
            return false;
        }

        public void Clear()
        {
            _dict.Clear();
            _pairs.Clear(); // 同时清空序列化字典
        }

        //键 索引器
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


        //列表 索引器
        public TValue this[int index]
        {
            get
            {
                if (index < 0 || index >= _pairs.Count)
                {
                    throw new IndexOutOfRangeException("索引超出范围。");
                }
                return _pairs[index].value;
            }
            set
            {
                if (index < 0 || index >= _pairs.Count)
                {
                    throw new IndexOutOfRangeException("索引超出范围。");
                }

                var key = _pairs[index].key;
                _dict[key] = value; // 更新字典中的值
                _pairs[index] = new SerializePair<Tkey, TValue>(key, value); // 更新序列化列表中的值
                _isDirty = true; // 标记字典已更改
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

        // 这里实现 GetEnumerator 方法
        public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        // 获取字典的 Keys
        public Dictionary<Tkey, TValue>.KeyCollection Keys => _dict.Keys;

        // 获取字典的 Values
        public Dictionary<Tkey, TValue>.ValueCollection Values => _dict.Values;

        public int Count => _pairs.Count;

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
