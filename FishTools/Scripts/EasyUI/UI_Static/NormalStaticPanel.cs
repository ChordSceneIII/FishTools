using FishTools;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 静态UI面板，不会销毁而是是隐藏，在运行场景中一直存在
/// </summary>

namespace EasyUI
{
    public class NormalStaticPanel : BaseStaticUI, IDragHandler
    {
        RectTransform thisRect;

        [Tooltip("是否可拖拽")] public bool canDrag = true;

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
