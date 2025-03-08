using System.Collections;
using System.Collections.Generic;
using FishTools.EasyUI;
using UnityEngine;

namespace FishTools.Tests
{
    public class TestInventory : BaseInventory<TestItemDATA, TestType, TestPath>
    {
        public void Add(TestType type, int amount)
        {
            if (amount == 0)
            {
                Debug.LogWarning("数量不能为0");
                return;
            }

            var newItem = FindItemByType<TestItem>(type);

            if (newItem != null)
            {
                newItem.AddCount(amount);
                return;
            }
            if (newItem == null)
            {
                var empty_index = FindEmptySlot();
                if (empty_index < 0)
                {
                    Debug.LogWarning("背包满了");
                    return;
                }
                if (empty_index >= 0)
                {
                    var item = AddItem<TestItem>(new TestItemDATA(amount, type), empty_index);
                }
            }
        }

        public void Remove(TestType type)
        {
            var item = FindItemByType<TestItem>(type);

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
