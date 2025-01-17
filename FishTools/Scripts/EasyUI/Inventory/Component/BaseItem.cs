using System;
using FishTools;
using UnityEngine;

/// <summary>
///  物品数据接口
/// </summary>

namespace FishTools.EasyUI
{
    public abstract class BaseItem<DATA, TYPE> : MonoBehaviour, IBaseItem where DATA : BaseItemDATA<TYPE> where TYPE : Enum
    {
        [Label("堆叠上限")] public int maxCount = 1;//最大数量。如果为1就是默认不叠加
        public DATA data;
        public int IMaxCount => maxCount;
        public int ICount
        {
            get
            {
                return data.Count;
            }
            set
            {
                data.Count = value;
            }
        }
        public Enum Type => data.type;

        public int SlotIndex
        {
            get
            {
                return data.slotIndex;
            }
        }

        public abstract void AddCount(int count);
    }

}