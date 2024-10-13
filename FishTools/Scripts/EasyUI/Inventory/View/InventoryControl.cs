using UnityEngine;
using FishToolsEditor;
using System.Collections.Generic;
using FishTools;
using System;

/// <summary>
/// 对于非本背包管理的数据类型，如果存在这么一个物体，那么就会在下次加载时被清空掉
/// </summary>

namespace EasyUI
{
    public class InventoryControl : MonoBehaviour
    {
        [Label("背包标签")] public string inventoryTag;
        [Label("物品移动")] public bool canDragItem = true;
        [Label("友好标签")]public List<string> friendTags;

        // 检查是否允许在背包之间流通物品
        public bool IsFriend(InventoryControl targetInventory)
        {
            // 检查目标背包是否在允许的访问标签集合内
            return friendTags.Contains(targetInventory.inventoryTag);
        }


    }
}
