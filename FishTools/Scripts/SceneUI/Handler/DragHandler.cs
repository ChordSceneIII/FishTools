using FishToolsEditor;
using UnityEngine;
/// <summary>
/// 拖拽控制器，比用OnMouseDrag更灵活
/// </summary>

namespace SceneUITool
{

    public class DragHandler : BaseUIHandler
    {
        public bool lockX; // 控制X轴锁定
        public bool lockY; // 控制Y轴锁定
        public bool lockZ = true; // 默认锁定Z轴

        // 新增的可选项
        [Label("透视修正")] public bool usePerspectiveDistance = true; // 是否计算透视距离
        [Label("旋转修正")] public bool useCameraRotation = true; // 是否计算相机旋转变换

        private Camera mainCamera;
        private Vector3 cachedCameraPos; // 缓存相机的位置
        private Quaternion cachedCameraRot; // 缓存相机的旋转
        private Vector3 originalPosition; // 原始物体位置

        private void Start()
        {
            mainCamera = Camera.main;
            CacheCameraTransform();
            originalPosition = transform.position; // 缓存物体原始位置
        }

        // 检查相机变换是否发生变化
        private bool HasCameraTransformChanged()
        {
            return cachedCameraPos != mainCamera.transform.position || cachedCameraRot != mainCamera.transform.rotation;
        }

        // 缓存相机的变换数据
        private void CacheCameraTransform()
        {
            cachedCameraPos = mainCamera.transform.position;
            cachedCameraRot = mainCamera.transform.rotation;
        }

        public void Drag()
        {
            // 如果相机变换发生了变化，更新缓存
            if (useCameraRotation && HasCameraTransformChanged())
            {
                CacheCameraTransform();
            }

            // 计算鼠标位置到世界坐标的转换，是否使用透视距离
            Vector3 mousePosition;
            if (usePerspectiveDistance)
            {
                mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                            -mainCamera.transform.position.z); // 考虑透视
            }
            else
            {
                mousePosition = Input.mousePosition; // 不考虑透视
            }

            var targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // 获取当前物体位置，用来处理锁定的轴
            Vector3 newPosition = transform.position;

            if (useCameraRotation)
            {
                // 计算相机旋转的影响，获取物体在相机本地空间中的位置
                Vector3 cameraRelativePos = mainCamera.transform.InverseTransformPoint(newPosition);

                // 将鼠标目标位置转换到相机的本地空间
                Vector3 localTargetPosition = mainCamera.transform.InverseTransformPoint(targetPosition);

                // 更新锁定轴的处理
                if (!lockX)
                    cameraRelativePos.x = localTargetPosition.x;

                if (!lockY)
                    cameraRelativePos.y = localTargetPosition.y;

                if (!lockZ)
                    cameraRelativePos.z = localTargetPosition.z;

                // 将本地坐标转换回世界坐标并应用
                newPosition = mainCamera.transform.TransformPoint(cameraRelativePos);
            }
            else
            {
                // 不考虑相机旋转的简单版本
                if (!lockX)
                    newPosition.x = targetPosition.x;

                if (!lockY)
                    newPosition.y = targetPosition.y;

                if (!lockZ)
                    newPosition.z = targetPosition.z;
            }

            // 强制保持锁定轴不变
            if (lockX)
                newPosition.x = originalPosition.x;

            if (lockY)
                newPosition.y = originalPosition.y;

            if (lockZ)
                newPosition.z = originalPosition.z;

            // 更新物体位置
            transform.position = newPosition;
        }
    }
}
