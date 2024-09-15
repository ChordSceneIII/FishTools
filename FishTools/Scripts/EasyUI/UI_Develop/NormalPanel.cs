using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyUI
{
    public class NormalPanel : BasePanel, IDragHandler
    {
        RectTransform thisRect;

        [Tooltip("是否可拖拽")]public bool canDrag = true;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (canDrag)
                thisRect.anchoredPosition += eventData.delta;
        }
    }
}
