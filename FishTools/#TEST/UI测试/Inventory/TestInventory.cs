using System.Collections;
using System.Collections.Generic;
using FishTools.EasyUI;
using UnityEngine;

namespace FishToolsDEMO
{
    public class TestInventory : BaseInventory<TestItemDATA, TestTypeEnum, TestType>
    {

        public void Add(TestTypeEnum type, int amount)
        {
            if (amount == 0)
            {
                Debug.LogWarning("数量不能为0");
                return;
            }

            (Slot, TestItem) value = FindUsedSlot<TestItem>(type);
            var newItem = value.Item2;

            if (newItem != null)
            {
                newItem.AddCount(amount);
                return;
            }
            if (newItem == null)
            {
                var empty_slot = FindEmptySlot();
                if (empty_slot == null)
                {
                    Debug.LogWarning("背包满了");
                    return;
                }
                if (empty_slot != null)
                {
                    var item = AddItem<TestItem>(new TestItemDATA(amount, type), SlotList.IndexOf(empty_slot));
                }
            }
        }

        public void Remove(TestTypeEnum type)
        {
            var slot = FindUsedSlot<TestItem>(type);
            TestItem item = slot.Item2;

            if (item != null)
            {
                item.TextCount.text = null;
                RemoveItem(item);
            }
            else
            {
                Debug.LogWarning($"{type} 道具不存在");
            }
        }
    }


}
