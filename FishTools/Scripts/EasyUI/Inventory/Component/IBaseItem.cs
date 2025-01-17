using System;
using UnityEngine;

namespace FishTools.EasyUI
{
    public interface IBaseItem
    {
        GameObject gameObject { get; }
        int IMaxCount { get; }
        int ICount { get; set; }
        int SlotIndex { get;}
        Enum Type { get; }
        void AddCount(int count);
    }
}