using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FishTools
{
    /// <summary>
    /// 映射表（一一对应）
    /// </summary>
    public class DMapper<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _keyToValue;
        private Dictionary<TValue, TKey> _valueToKey;

        public DMapper()
        {
            _keyToValue = new Dictionary<TKey, TValue>();
            _valueToKey = new Dictionary<TValue, TKey>();
        }

        public DMapper(DMapper<TKey, TValue> other)
        {
            _keyToValue = new Dictionary<TKey, TValue>(other._keyToValue);
            _valueToKey = new Dictionary<TValue, TKey>(other._valueToKey);
        }

        public void Add(TKey key, TValue value)
        {
            if (_keyToValue.ContainsKey(key))
            {
                Debug.LogWarning($"Key '{key}' already exists in the mapper.");
                return;
            }

            if (_valueToKey.ContainsKey(value))
            {
                Debug.LogWarning($"Value '{value}' already exists in the mapper.");
                return;
            }

            _keyToValue[key] = value;
            _valueToKey[value] = key;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _keyToValue.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return _valueToKey.TryGetValue(value, out key);
        }

        public bool ContainsKey(TKey key)
        {
            return _keyToValue.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _valueToKey.ContainsKey(value);
        }

        public void RemoveByKey(TKey key)
        {
            if (_keyToValue.TryGetValue(key, out var value))
            {
                _keyToValue.Remove(key);
                _valueToKey.Remove(value);
            }
        }

        public void RemoveByValue(TValue value)
        {
            if (_valueToKey.TryGetValue(value, out var key))
            {
                _valueToKey.Remove(value);
                _keyToValue.Remove(key);
            }
        }

        public List<TValue> ValueToList()
        {
            return _keyToValue.Values.ToList();
        }
        public List<TKey> KeyToList()
        {
            return _keyToValue.Keys.ToList();
        }


        /// <summary>
        /// 交换映射
        /// </summary>
        public void Swap(TValue vlaueA, TValue valueB)
        {
            if (_valueToKey.TryGetValue(vlaueA, out var keyA) && _valueToKey.TryGetValue(valueB, out var keyB))
            {
                _keyToValue[keyA] = valueB;
                _keyToValue[keyB] = vlaueA;

                _valueToKey[valueB] = keyA;
                _valueToKey[vlaueA] = keyB;
            }
        }

        public void Swap(TKey keyA, TKey keyB)
        {
            if (_keyToValue.TryGetValue(keyA, out var valueA) && _keyToValue.TryGetValue(keyB, out var valueB))
            {
                _keyToValue[keyA] = valueB;
                _keyToValue[keyB] = valueA;

                _valueToKey[valueB] = keyA;
                _valueToKey[valueA] = keyB;
            }
        }

        /// <summary>
        /// 打乱映射表
        /// </summary>
        public static DMapper<TKey, TValue> Shuffle(DMapper<TKey, TValue> target, int seed)
        {
            var keys = target._keyToValue.Keys.ToList();
            var values = target._keyToValue.Values.ToList();

            FMath.Shuffle(keys, seed);

            DMapper<TKey, TValue> newMapper = new DMapper<TKey, TValue>();

            for (int i = 0; i < keys.Count; i++)
            {
                newMapper.Add(keys[i], values[i]);
            }

            return newMapper;
        }


        public void Clear()
        {
            _keyToValue.Clear();
            _valueToKey.Clear();
        }
    }
}
