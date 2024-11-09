using UnityEngine;
using UnityEngine.UI;
using FishTools;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(RectTransform))]
    public class SlotGroup : LayoutGroup
    {

        [SerializeField, Label("锁定")] private bool isLocked = false;
        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
            set
            {
                isLocked = value;
                this.GetComponent<SlotOverImage>()?.KeepLayer();
            }
        }

        [SerializeField, Label("选择")]
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                this.GetComponent<SlotSelectImage>()?.KeepLayer();
            }
        }
        public Vector2 itemSize = new Vector2(1.0f, 1.0f);
        [SerializeField] private GameObject itemObj;
        public GameObject ItemObj
        {
            get
            {
                if (itemObj == null)
                {
                    itemObj = transform.GetComponentInChildren<IBaseItemData>()?.gameObject;
                }
                return itemObj;
            }
        }
        public int layer = 0;

        //Item
        private RectTransform itemRect;
        public RectTransform ItemRect
        {
            get
            {
                if (itemRect == null)
                {
                    var obj = transform.GetComponentInChildren<IBaseItemData>()?.gameObject;
                    itemRect = obj?.GetComponent<RectTransform>();
                }
                return itemRect;
            }
        }

        private void UpdateLayout()
        {
            // 确保 Item 存在
            if (ItemRect != null)
            {
                KeepGridCenter(ItemRect, itemSize);
                ItemRect.SetSiblingIndex(layer);
            }
        }

        internal void KeepGridCenter(RectTransform rect, Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(rectTransform.rect.width * size.x, rectTransform.rect.height * size.y);
        }

        public override void CalculateLayoutInputVertical()
        {
            UpdateLayout();
        }
        public override void SetLayoutHorizontal()
        {
            UpdateLayout();
        }
        public override void SetLayoutVertical()
        {
            UpdateLayout();
        }
    }
}
