using UnityEngine;

namespace EasyUI
{
    public class Inventory : MonoBehaviour
    {
        [Tooltip("是否上锁（禁止跨容器移动）")] public bool isLocked = false;

        [Tooltip("锁定物体拖拽")] public bool lockDragItem = false;
    }
}
