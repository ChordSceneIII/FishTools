using UnityEngine;
using UnityEngine.EventSystems;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 跟随控制
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class FollowHandler : BaseHandler, IDragHandler
    {
        private Camera mycamera;
        public Camera MyCamera
        {
            get
            {
                if (mycamera == null) mycamera = Camera.main;
                return mycamera;
            }
        }

        [Label("跟随")] public bool isFollow = false;
        [Label("目标"), ConditionalField("isFollow", true)] public Transform targetTrans;//跟随对象
        [Label("偏移"), ConditionalField("isFollow", true)] public Vector3 offsetVector;
        [Label("平滑"), ConditionalField("isFollow", true), Range(0f, 1)] public float smoothTime = 0.3f;
        [Label("拖拽")] public bool drag = false;
        [Label("限制在屏幕范围内")] public bool keepinScreen = false;
        private Canvas canvas;
        [SerializeField] private RectTransform canvas_rectTrans;
        private RectTransform _rectTransform;
        public RectTransform rectTransform => FishUtility.LazyGet(this, ref _rectTransform);
        /// <summary>
        /// 设置跟随目标
        /// </summary>
        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            canvas_rectTrans = canvas?.GetComponent<RectTransform>();
        }
        public void SetTarget(Transform target)
        {
            targetTrans = target;
            transform.position = TargetPos();
        }

        public void OnEnable()
        {
            if (isFollow) transform.position = TargetPos();
        }

        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();
            canvas = GetComponentInParent<Canvas>();
            canvas_rectTrans = canvas?.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (drag&&interactable)
            {
                if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    transform.position += (Vector3)eventData.delta;
                }
                else
                {
                    transform.position += ScreenToWorldDelta(eventData.delta, Camera.main);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            UpdatePos();
            KeepInScreen();
        }

        /// <summary>
        /// 更新位置
        /// </summary>
        // 更新位置
        public void UpdatePos()
        {
            if (interactable)
            {
                if (isFollow && targetTrans != null && smoothTime > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, TargetPos(), smoothTime);
                }
            }

        }

        /// <summary>
        /// 限制在画布内
        /// </summary>
        public void KeepInScreen()
        {
            if (keepinScreen && canvas_rectTrans != null)
            {
                // UI 的真实坐标
                var pos = rectTransform.position;

                // UI 的尺寸
                var sizeLD = rectTransform.sizeDelta * rectTransform.pivot;
                var sizeRU = rectTransform.sizeDelta * (Vector2.one - rectTransform.pivot);

                // Canvas画布尺寸
                float xDistance = canvas_rectTrans.rect.width / 2;
                float yDistance = canvas_rectTrans.rect.height / 2;

                //尺寸乘以缩放比例得到实际尺寸
                var minX = canvas_rectTrans.position.x + (-xDistance + sizeLD.x) * canvas_rectTrans.lossyScale.x;
                var maxX = canvas_rectTrans.position.x + (xDistance - sizeRU.x) * canvas_rectTrans.lossyScale.x;
                var minY = canvas_rectTrans.position.y + (-yDistance + sizeLD.y) * canvas_rectTrans.lossyScale.y;
                var maxY = canvas_rectTrans.position.y + (yDistance - sizeRU.y) * canvas_rectTrans.lossyScale.y;

                // 限制 UI 坐标最大最小值
                float x = Mathf.Clamp(pos.x, minX, maxX);
                float y = Mathf.Clamp(pos.y, minY, maxY);

                // 调整 UI 坐标
                rectTransform.position = new Vector2(x, y);
            }
        }

        private Vector3 ScreenToWorldDelta(Vector2 screenDelta, Camera camera)
        {
            if (camera == null)
            {
                Debug.LogError("摄像机未设置！");
                return Vector3.zero;
            }

            // 将屏幕坐标增量转换为世界坐标增量
            Vector3 screenPos1 = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            Vector3 screenPos2 = camera.ScreenToWorldPoint(new Vector3(screenDelta.x, screenDelta.y, camera.nearClipPlane));

            return screenPos2 - screenPos1;
        }

        public Vector3 TargetPos()
        {
            if (targetTrans != null && canvas != null)
            {
                var targetPos = targetTrans.position + offsetVector;

                // 屏幕覆盖模式
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    targetPos = MyCamera.WorldToScreenPoint(targetPos);
                }
                return targetPos;
            }
            return transform.position;
        }
    }
}
