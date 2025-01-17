using UnityEngine;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 跟随控制
    /// </summary>
    public sealed class FollowHandler : BaseHandler
    {
        public Camera mycamera;
        public Camera MyCamera
        {
            get
            {
                if (mycamera == null) mycamera = Camera.main;
                return mycamera;
            }
        }

        [Label("目标")] public Transform targetTrans;//跟随对象
        [Label("偏移")] public Vector3 offsetVector;
        [Label("平滑"), Range(0f, 1)] public float smoothTime = 0.3f;
        [Label("覆盖模式")] public bool overlay = true;

        public void Init(Transform target)
        {
            targetTrans = target;
            SetPosition();
        }

        private void OnEnable()
        {
            SetPosition();
        }

        protected override void Update()
        {
            base.Update();
            UpdatePosition();
        }

        /// <summary>
        /// 更新位置
        /// </summary>
        public void UpdatePosition()
        {
            if (interactable && targetTrans != null && smoothTime > 0)
            {
                if (overlay)
                {
                    // 屏幕覆盖模式
                    Vector3 screenPosition = MyCamera.WorldToScreenPoint(targetTrans.position + offsetVector);
                    transform.position = Vector3.Lerp(transform.position, screenPosition, smoothTime);
                }
                else
                {
                    // 更新 FollowPanel 的位置
                    transform.position = Vector3.Lerp(transform.position, targetTrans.position + offsetVector, smoothTime);
                }

            }
        }

        public void SetPosition()
        {
            if (targetTrans != null)
            {
                if (overlay)
                {
                    // 屏幕覆盖模式
                    Vector3 screenPosition = MyCamera.WorldToScreenPoint(targetTrans.position + offsetVector);
                    transform.position = screenPosition;
                }
                else
                {
                    transform.position = targetTrans.position + offsetVector;

                }
            }
        }
    }
}
