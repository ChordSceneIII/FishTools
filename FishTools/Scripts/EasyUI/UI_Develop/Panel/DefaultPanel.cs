using FishToolsEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyUI
{
    public class DefaultPanel : BasePanel, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        RectTransform thisRect;

        [Label("拖拽")] public bool canDrag = true;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
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
            thisRect.SetAsLastSibling();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            thisRect.SetAsLastSibling();

        }
    }
}
