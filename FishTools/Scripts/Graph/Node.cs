using System;
using UnityEngine;

namespace FishTools.FGraph
{
    [Serializable]
    public class Node<T>
    {
        [SerializeField] private T data;
        public Node(T v)
        {
            data = v;
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}