using System.Collections.Generic;
using UnityEngine;

namespace FishTools.EasyUI
{
    public interface IBaseInventory
    {
        public GameObject gameObject { get; }
        List<Slot> SlotList { get; }
        void AddSlot();
        void RemoveSlot();
        void UpdateBag();
        void UpdateSlotList();
        Slot FindEmptySlot();
    }
}