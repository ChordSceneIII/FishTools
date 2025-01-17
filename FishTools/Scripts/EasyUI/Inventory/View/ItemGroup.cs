using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(RectTransform))]
    public class ItemGroup : LayoutGroup
    {
        [Label("相对父级尺寸")] public Vector2 size = new Vector2(1, 1);
        [Label("始终保持布局")] public bool alwaysKeepGrid;
        public void UpdateLayout()
        {
            var parent = rectTransform.parent as RectTransform;
            if (parent == null)
            {
                Debug.Log("父对象没有RectTransform组件");
                return;
            }

            KeepCenter(rectTransform, parent, size);
            rectTransform.SetAsFirstSibling();
        }

        internal void KeepCenter(RectTransform rect, RectTransform rectParent, Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(rectParent.rect.width * size.x, rectParent.rect.height * size.y);
        }
        protected override void OnEnable()
        {
            UpdateLayout();
        }

        public override void CalculateLayoutInputVertical()
        {
            if (alwaysKeepGrid)
                UpdateLayout();
        }

        public override void SetLayoutHorizontal()
        {
            if (alwaysKeepGrid)
                UpdateLayout();
        }

        public override void SetLayoutVertical()
        {
            if (alwaysKeepGrid)
                UpdateLayout();
        }
    }
}