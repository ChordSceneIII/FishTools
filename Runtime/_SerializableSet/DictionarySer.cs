using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        public DictionarySer()
        { }

        public DictionarySer(Dictionary<TKey, TValue> new_dic)
        {
            Clear();
            foreach (var kvp in new_dic)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        // 序列化列表 (只用于显示和序列化)
        [SerializeField]
        private List<SerializePair<TKey, TValue>> _pairs = new List<SerializePair<TKey, TValue>>();

        // 实际字典 (用于所有增删改查操作)
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

        [SerializeField] private bool _isRepeatKey = false;

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
                    _isRepeatKey = true;
                    return; // 退出序列化
                }
            }
            _isRepeatKey = false;
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
                if (!_dict.TryGetValue(key, out TValue value))
                    DebugF.LogError($"Key {key} 不存在于 Dictionary 中。");
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
        public void CopyFromDic(Dictionary<TKey, TValue> dict)
        {
            Clear();
            foreach (var kvp in dict)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        public Dictionary<TKey, TValue> ToDic()
        {
            Dictionary<TKey, TValue> new_dic = new Dictionary<TKey, TValue>();

            foreach (var kvp in _dict)
            {
                new_dic.Add(kvp.Key, kvp.Value);
            }
            return new_dic;
        }


        /// <summary>
        /// 浅拷贝
        /// </summary>
        public void Copy(DictionarySer<TKey, TValue> new_dic)
        {
            Clear();
            foreach (var kvp in new_dic)
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

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DictionarySer<,>))]
    public class DictionarySerDrawer : PropertyDrawer
    {
        private const float KeyValueRatio = 0.45f;
        private const float ButtonWidth = 20f;
        private const float LineSpacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 获取关键属性
            SerializedProperty pairsProp = property.FindPropertyRelative("_pairs");
            SerializedProperty isRepeatKeyProp = property.FindPropertyRelative("_isRepeatKey");

            // 计算布局
            Rect currentRect = new Rect(position)
            {
                height = EditorGUIUtility.singleLineHeight
            };

            // 显示重复键警告
            if (isRepeatKeyProp.boolValue)
            {
                EditorGUI.HelpBox(currentRect, "存在重复的 Key！请确保所有键的唯一性。", MessageType.Warning);
                currentRect.y += currentRect.height + LineSpacing;
            }

            // 绘制列表标签
            EditorGUI.LabelField(currentRect, label);
            currentRect.y += currentRect.height + LineSpacing;

            // 绘制每个键值对
            for (int i = 0; i < pairsProp.arraySize; i++)
            {
                SerializedProperty pairProp = pairsProp.GetArrayElementAtIndex(i);
                DrawKeyValuePair(ref currentRect, pairsProp, pairProp, i);
            }

            // 添加新元素按钮
            DrawAddButton(ref currentRect, pairsProp);

            EditorGUI.EndProperty();
        }

        private void DrawKeyValuePair(ref Rect rect, SerializedProperty listProp, SerializedProperty pairProp, int index)
        {
            Rect lineRect = new Rect(rect)
            {
                height = EditorGUIUtility.singleLineHeight
            };

            // 元素索引标签
            Rect indexRect = new Rect(lineRect)
            {
                width = 30f
            };
            EditorGUI.LabelField(indexRect, $"[{index}]");
            lineRect.x += indexRect.width;
            lineRect.width -= indexRect.width + ButtonWidth;

            // Key/Value 水平布局
            float keyWidth = lineRect.width * KeyValueRatio;
            float valueWidth = lineRect.width - keyWidth;

            // Key 字段
            Rect keyRect = new Rect(lineRect)
            {
                width = keyWidth - LineSpacing
            };
            EditorGUI.PropertyField(keyRect, pairProp.FindPropertyRelative("key"), GUIContent.none);

            // Value 字段
            Rect valueRect = new Rect(keyRect)
            {
                x = keyRect.x + keyRect.width + LineSpacing,
                width = valueWidth - LineSpacing
            };
            EditorGUI.PropertyField(valueRect, pairProp.FindPropertyRelative("value"), GUIContent.none);

            // 删除按钮
            Rect buttonRect = new Rect(lineRect)
            {
                x = lineRect.x + lineRect.width,
                width = ButtonWidth
            };
            if (GUI.Button(buttonRect, "-"))
            {
                listProp.DeleteArrayElementAtIndex(index);
            }

            rect.y += lineRect.height + LineSpacing;
        }

        private void DrawAddButton(ref Rect rect, SerializedProperty listProp)
        {
            Rect buttonRect = new Rect(rect)
            {
                width = 60f,
                height = EditorGUIUtility.singleLineHeight
            };

            if (GUI.Button(buttonRect, "+ Add"))
            {
                listProp.arraySize++;
            }

            rect.y += buttonRect.height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;

            // 错误提示高度
            SerializedProperty isRepeatKeyProp = property.FindPropertyRelative("_isRepeatKey");
            if (isRepeatKeyProp.boolValue)
            {
                height += EditorGUIUtility.singleLineHeight + LineSpacing;
            }

            // 标题高度
            height += EditorGUIUtility.singleLineHeight + LineSpacing;

            // 每个元素的高度
            SerializedProperty pairsProp = property.FindPropertyRelative("_pairs");
            height += pairsProp.arraySize * (EditorGUIUtility.singleLineHeight + LineSpacing);

            // 添加按钮高度
            height += EditorGUIUtility.singleLineHeight;

            return height;
        }
    }
#endif
}


