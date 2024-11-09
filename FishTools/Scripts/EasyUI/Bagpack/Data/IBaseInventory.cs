using UnityEngine;

namespace  FishTools.EasyUI
{
    public interface IBaseInventory
    {
        public GameObject gameObject { get; }
        public DictionarySerializable<string, GameObject> SlotDic { get; set; }
        void AddSlot();
        void RemoveSlot();
        void UpdateSlots();
        void UpdateItems();
        GameObject FindEmptySlot();
        void ClearInventory();
    }
}