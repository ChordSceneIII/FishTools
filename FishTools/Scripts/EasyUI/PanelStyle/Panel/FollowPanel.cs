using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 跟随面板
/// </summary>

namespace FishTools.EasyUI
{
    public class FollowPanel : BasePanel
    {
        [Label("跟随目标")] public Transform targetTrans;//跟随对象
        [Label("一直跟随")] public bool alwaysFollow = false;
        [Label("偏移")] public Vector3 offsetVector;
        [Label("点击外部关闭")] public bool autoCloseOutside = false;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetTarget(Transform target)
        {
            targetTrans = target;
        }

        private void OnEnable()
        {
            //打开时更新位置
            UpdatePosition();
        }

        private void Update()
        {

            if (alwaysFollow)
            {
                UpdatePosition();
            }

            if (Input.GetMouseButtonDown(0))
            {
                CloseWhenOutside();
            }

        }

        // 更新 FollowPanel 的位置的方法
        private void UpdatePosition()
        {
            if (targetTrans != null)
            {
                // 将世界坐标转换为屏幕坐标
                Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTrans.position + offsetVector);

                // 更新 FollowPanel 的位置
                rectTransform.position = screenPosition;
            }
        }

        // 检查点击是否在面板外部
        private void CloseWhenOutside()
        {
            // 执行射线检测
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            var raycastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            // 检查射线检测结果中是否包含面板的 RectTransform
            foreach (var result in raycastResults)
            {
                if (result.gameObject == this.gameObject)
                {
                    return; // 如果点击在面板上，则直接返回
                }
            }

            //如果点击不在面板上则关闭面板
            this.gameObject.SetActive(false);
        }
    }
}
