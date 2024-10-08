using FishToolsEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyUI
{
    public class DefaultPanel : BasePanel, IDragHandler
    {
        RectTransform thisRect;

        [Label("拖拽")]public bool canDrag = true;

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
