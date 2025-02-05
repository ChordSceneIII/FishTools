using System.Collections.Generic;
using UnityEngine;

namespace FishTools.EasyUI
{
    public interface IBaseInventory
    {
        public GameObject gameObject { get; }
        List<Slot> Slots { get; }
        Slot AddSlot(string name, bool isLocked);
        void RemoveSlot();
        void UpdateList();
        void RemoveItem(int index);
        bool IsFull();
        void Cache();
        void ReloadCache();
        void Clear();
        int FindEmptySlot(bool includeLocked = false);
    }
}