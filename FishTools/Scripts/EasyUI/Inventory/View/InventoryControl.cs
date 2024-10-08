using UnityEngine;
using FishToolsEditor;
using System.Collections.Generic;
using FishTools;
using System;

namespace EasyUI
{
    public class InventoryControl : MonoBehaviour
    {
        [Label("背包标识")] public string inventoryTag;
        [Label("上锁")] public bool isLocked = false;
        [Label("锁定拖拽")] public bool lockDragItem = false;

        //TODO:只需要规定好背包通信即可，即管理同一类型的数据的背包既可相互移动，这需要手动来设置的。
        //对于非本背包管理的数据类型，如果存在这么一个物体，那么就会在下次加载时被清空掉
    }
}
