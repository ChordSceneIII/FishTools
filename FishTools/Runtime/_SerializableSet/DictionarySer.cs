using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class DictionarySer<TKey, TValue> : ISerializationCallbackReceiver
    {
        // 序列化列表 (只用于显示和序列化)
        [SerializeField]
        private List<SerializePair<TKey, TValue>> _pairs = new List<SerializePair<TKey, TValue>>();

        // 实际字典 (用于所有增删改查操作)
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

        //序列化之前检查键的唯一性
        public void OnBeforeSerialize()
        {
            // 检查键的唯一性
            HashSet<TKey> keySet = new HashSet<TKey>();

            foreach (var pair in _pairs)
            {
                if (!keySet.Add(pair.key))
                {
                    // 发现重复的键，提示用户
                    DebugF.LogError($"key重复{pair.key}");
                }
            }
        }

        //序列化结束后同步字典
        public void OnAfterDeserialize()
        {
            _dict.Clear();
            foreach (var pair in _pairs)
            {
                _dict[pair.key] = pair.value;
            }
        }

        /// <summary>
        /// 新增键值对 key不能重复
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (_dict.ContainsKey(key))
            {
                DebugF.LogError("Key已经存在Dicitonary中");
                return;
            }

            _dict.Add(key, value);
            _pairs.Add(new SerializePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// 移除（通过key）
        /// </summary>
        public bool Remove(TKey key)
        {
            if (_dict.Remove(key))
            {
                _pairs.RemoveAll(pair => EqualityComparer<TKey>.Default.Equals(pair.key, key));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在指定位置插入
        /// </summary>
        public void Insert(int index, TKey key, TValue value)
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
            _pairs.Insert(index, new SerializePair<TKey, TValue>(key, value));

            // 更新字典
            _dict[key] = value; // 添加到字典

        }

        /// <summary>
        /// 修改Key值
        /// </summary>
        public bool UpdateKey(TKey oldKey, TKey newKey)
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
                    if (EqualityComparer<TKey>.Default.Equals(_pairs[i].key, oldKey))
                    {
                        // 更新键
                        _pairs[i] = new SerializePair<TKey, TValue>(newKey, value);
                        break;
                    }
                }
                // 添加新的键值对
                _dict.Add(newKey, value);
                return true;
            }

            // 返回 false 表示旧键不存在
            return false;
        }

        /// <summary>
        /// 清除值null引用或Missing引用
        /// </summary>
        public void ClearNullValues()
        {
            List<TKey> keysToRemove = new List<TKey>();

            // 遍历列表，查找值为 null 的键
            foreach (var pair in _dict)
            {
                if (pair.Value == null || pair.Value.Equals(null))
                {
                    keysToRemove.Add(pair.Key);
                }
            }

            // 移除值为 null 的键值对
            foreach (var key in keysToRemove)
            {
                Remove(key);
            }
            // DebugF.Log($"已清除 {keysToRemove.Count} 个值为 null 的键值对。");
        }

        public void Clear()
        {
            _dict.Clear();
            _pairs.Clear(); // 同时清空序列化字典
        }

        //键 索引器
        public TValue this[TKey key]
        {
            get
            {
                _dict.TryGetValue(key, out TValue value);
                return value;
            }
            set
            {
                if (_dict.ContainsKey(key))
                {
                    _dict[key] = value;
                    // 更新 _pairs 中的值
                    for (int i = 0; i < _pairs.Count; i++)
                    {
                        if (EqualityComparer<TKey>.Default.Equals(_pairs[i].key, key))
                        {
                            _pairs[i] = new SerializePair<TKey, TValue>(key, value);
                        }
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public TValue Index(int index)
        {
            return _pairs[index].value;
        }

        // public TValue this[int index]
        // {
        //     get => _pairs[index].value;
        //     set => _pairs[index] = new SerializePair<TKey, TValue>(_pairs[index].key, value);
        // }

        public bool TryGetValue(TKey key, out TValue value) => _dict.TryGetValue(key, out value);

        public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

        // 这里实现 GetEnumerator 方法 用于遍历kvps
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        // 获取字典的 Keys
        public Dictionary<TKey, TValue>.KeyCollection Keys => _dict.Keys;

        // 获取字典的 Values
        public Dictionary<TKey, TValue>.ValueCollection Values => _dict.Values;

        public List<SerializePair<TKey, TValue>> List => _pairs;

        public int Count => _pairs.Count;

        /// <summary>
        /// 从 Dictionary 转换到 DictionarySerializable
        /// </summary>
        public void Copy(Dictionary<TKey, TValue> dict)
        {
            Clear();
            foreach (var kvp in dict)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
        public void Copy(DictionarySer<TKey, TValue> dict)
        {
            Clear();
            foreach (var kvp in dict)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// 把Dictionary的数据合并到 DictionarySerializable, 如果有相同key会被覆盖
        /// </summary>
        public void MergeDATA(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                this[kvp.Key] = kvp.Value;
            }
        }


        /// <summary>
        /// 把DictionarySer的数据合并, 如果有相同key会被覆盖
        /// </summary>
        public void MergeDATA(DictionarySer<TKey, TValue> dictser)
        {
            foreach (var kvp in dictser)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

    }
}
