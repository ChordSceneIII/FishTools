using UnityEngine;
using UnityEngine.EventSystems;
using FishToolsEditor;

namespace EasyUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FollowPanel : BasePanel
    {
        [ReadOnly, SerializeField] private Transform targetTrans;//跟随对象
        public Transform TargetTrans
        {
            get
            {
                return targetTrans;
            }
            set
            {
                targetTrans = value;
            }
        }
        [SerializeField] internal bool alwaysFollow = false;
        [SerializeField] internal Vector3 offsetVector;
        [SerializeField] internal bool autoCloseOutside = false;
        private RectTransform rectTransform;

        //UI组件全局设置
        private CanvasGroup canvasGroup;

        private bool firstGetTarget = true;
        private void Awake()
        {
            // 在 Awake 中获取 RectTransform 组件并缓存，避免重复调用
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;//隐藏组件
        }

        private void Update()
        {
            // 在第一次获取到target对象时立即同步位置
            if (targetTrans != null && firstGetTarget)
            {
                UpdatePosition();
                firstGetTarget = false;
                canvasGroup.alpha = 1;//更新完位置后才显示
            }

            // 在每帧更新位置，如果 `alwaysFollow` 为 true
            if (alwaysFollow && targetTrans != null)
            {
                UpdatePosition();
            }
        }

        // 更新 FollowPanel 的位置的方法
        private void UpdatePosition()
        {
            // 将世界坐标转换为屏幕坐标
            Vector3 screenPosition = RectTransformUtility
            .WorldToScreenPoint(Camera.main, targetTrans.position + offsetVector);

            // 更新 FollowPanel 的位置
            rectTransform.position = screenPosition;

            // 检查点击是否在面板外部
            if (autoCloseOutside && Input.GetMouseButtonDown(0))
            {
                CheckClickOutsidePanel();
            }
        }

        // 检查点击是否在面板外部
        private void CheckClickOutsidePanel()
        {
            // 创建一个 PointerEventData 对象
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            // 创建一个用于存放结果的列表
            var raycastResults = new System.Collections.Generic.List<RaycastResult>();

            // 执行射线检测
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            // 检查射线检测结果中是否包含面板的 RectTransform
            foreach (var result in raycastResults)
            {
                if (result.gameObject == gameObject)
                {
                    return; // 如果点击在面板上，直接返回
                }
            }

            var refID = this.GetInstanceID();
            var isInPool = UIManager_pool.Instance.isRegistered(refID);
            // 如果点击不在面板上，关闭面板

            if (isInPool)
            {
                UIManager_pool.Instance.ClosePanel(refID);
            }
            else
            {
                UIManager_develop.Instance.ClosePanel(this.panelname);
            }
        }
    }
}
