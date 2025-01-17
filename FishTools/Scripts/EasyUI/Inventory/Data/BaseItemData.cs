using System;
using FishTools;
using UnityEngine;

namespace FishTools.EasyUI
{
    [Serializable]
    public abstract class BaseItemDATA<TYPE> where TYPE : Enum
    {
        // public abstract BaseItemType<T> Type { get; set; }  //类型
        public TYPE type;
        [ReadOnly] public int slotIndex; //slot的索引映射
        [SerializeField] protected int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                if (count < 0) count = 0;
            }
        }
    }

    //利用ScriptObject 加 DictionarySerialize 来存储路径关系从而脱离使用Path的限制
}
