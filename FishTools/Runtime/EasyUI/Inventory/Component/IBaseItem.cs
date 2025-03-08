using System;
using UnityEngine;

namespace FishTools.EasyUI
{
    public interface IBaseItem
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        int ICount { get; set; }
        int SlotIndex { get; }
        Slot OfSlot { get; }
        Enum Type { get; }
    }
}