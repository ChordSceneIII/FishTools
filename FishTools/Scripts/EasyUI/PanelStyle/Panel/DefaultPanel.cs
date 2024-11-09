using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 默认面板 交互样式
/// </summary>

namespace FishTools.EasyUI
{
    public class DefaultPanel : BasePanel, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        RectTransform thisRect;

        [Label("拖拽")] public bool canDrag = false;

        [Label("聚焦")] public bool focus = false;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            if (focus)
                thisRect.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
            {
                thisRect.anchoredPosition += eventData.delta;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (focus)
                thisRect.SetAsLastSibling();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (focus)
                thisRect.SetAsLastSibling();
        }
    }
}
