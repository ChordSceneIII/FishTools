using UnityEngine;

/// <summary>
/// 拖拽控制器，比用OnMouseDrag更灵活更快
/// </summary>

namespace SceneUITool
{
    public class DragHandler : MonoBehaviour
    {
        public bool lockX;
        public bool lockY;
        public bool lockZ = true;

        public void Drag(Vector3 position)
        {
            Vector3 newPosition = transform.position;

            if (!lockX)
                newPosition.x = position.x;

            if (!lockY)
                newPosition.y = position.y;

            if (!lockZ)
                newPosition.z = position.z;

            transform.position = newPosition;
        }
    }
}
