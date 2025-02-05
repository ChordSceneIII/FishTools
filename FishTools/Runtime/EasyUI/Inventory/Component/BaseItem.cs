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
        public DATA data;
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
                return data.slotindex;
            }
        }

        public Slot OfSlot
        {
            get
            {
                return GetComponentInParent<Slot>(true);
            }
        }

        public abstract void AddCount(int count);
    }

}