using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 唯一顺序集，基于hash查找。元素唯一，且新加元素会被移到末尾。
/// </summary>


namespace FishTools
{
    [Serializable]
    public class ListOrdered<T>
    {
        [SerializeField]
        private List<T> _list = new List<T>(); // 用于序列化和顺序存储

        private HashSet<T> _hash = new HashSet<T>(); // 用于快速查找

        /// <summary>
        /// 添加元素。如果元素已存在，则将其移到末尾；如果不存在，则添加到末尾。
        /// </summary>
        public void Add(T item)
        {
            if (_hash.Contains(item))
            {
                // 如果元素已存在，则将其移到末尾
                _list.Remove(item);
                _list.Add(item);
            }
            else
            {
                // 如果元素不存在，则添加到末尾
                _list.Add(item);
                _hash.Add(item);
            }
        }

        /// <summary>
        /// 移除元素。
        /// </summary>
        public bool Remove(T item)
        {
            if (_hash.Contains(item))
            {
                _list.Remove(item);
                _hash.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查元素是否存在。
        /// </summary>
        public bool Contains(T item)
        {
            return _hash.Contains(item);
        }

        /// <summary>
        /// 获取元素的索引。如果元素不存在，返回 -1。
        /// </summary>
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }


        /// <summary>
        /// 清空链表。
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            _hash.Clear();
        }

        public T Last()
        {
            if (_list.Count == 0)
                return default;
            return _list[_list.Count - 1];
        }

        public T First()
        {
            if (_list.Count == 0)
                return default;
            return _list[0];
        }

        public void MoveAt(T item, int index)
        {
            if (index >= _list.Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (_hash.Contains(item))
            {
                _list.Remove(item);
                _list.Insert(index, item);
            }
            else
            {
                _hash.Add(item);
                _list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            if (index < _list.Count)
            {
                _hash.Remove(_list[index]);
                _list.RemoveAt(index);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 获取链表中的元素数量。
        /// </summary>
        public int Count => _list.Count;

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    }
}