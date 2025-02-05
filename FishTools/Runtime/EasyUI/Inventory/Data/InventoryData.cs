using System;
using System.Collections.Generic;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 背包数据
    /// </summary>
    [Serializable]
    public class InventoryData<DATA>
    {
        public InventoryData() { }
        public InventoryData(int slot_count, List<int> locked_Index, List<DATA> items)
        {
            this.slot_count = slot_count;
            this.locked_Index = locked_Index;
            this.items = items;
        }

        public int slot_count = 0; //背包大小
        public List<int> locked_Index = new List<int>(); //锁定格子序列
        public List<DATA> items = new List<DATA>(); // 物品数据
    }
}