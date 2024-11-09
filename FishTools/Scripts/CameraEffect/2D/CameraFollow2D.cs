using UnityEngine;

namespace FishTools.CameraEffect
{
    public class CameraFollow2D : MonoBehaviour
    {
        [Label("相机")] public Transform targetCamera;
        [Label("跟随目标")] public Transform target;

        [Label("平滑"), Range(0.0f, 1.0f)] public float smoothSpeed = 0.05f;
        public Vector3 offset;

        void LateUpdate()
        {
            if (target == null || targetCamera == null)
            {
                DebugF.LogError("未设置相机或跟随目标");
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(targetCamera.position, desiredPosition, smoothSpeed);
            targetCamera.position = smoothedPosition;

        }

#if UNITY_EDITOR
        [ContextMenu("预览")]
        void PreView()
        {
            targetCamera.position = target.position + offset;
        }
#endif
    }
}
