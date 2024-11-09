using FishTools;
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(SlotGroup))]
    public class SlotSelectImage : View
    {
        private SlotGroup slotHandler;
        private SlotGroup SlotHandler
        {
            get
            {
                if (slotHandler == null)
                {
                    slotHandler = GetComponent<SlotGroup>();
                }
                return slotHandler;
            }
        }

        [SerializeField, ReadOnly] private RectTransform selectimageRect;
        [Label("尺寸")] public Vector2 selectimageSize = new Vector2(1.0f, 1.0f);
        [Label("图片")] public Sprite selectimageSprite;
        [Label("材质")] public Material selectimageMaterial;
        [Label("颜色")] public Color selectimageColor = Color.white;
        [Label("层级")] public int layer = 0;


        public RectTransform SelectimageRect
        {
            get
            {
                if (selectimageRect != null && selectimageRect.parent != this.transform)
                {
                    DestroyImmediate(selectimageRect.gameObject);
                }

                if (selectimageRect == null)
                {
                    var obj = transform.Find("selectimage")?.gameObject;

                    if (obj == null)
                    {
                        obj = ViewUtils.CreateNewImage("selectimage", transform, selectimageSprite, selectimageColor, selectimageSize, selectimageMaterial);
                    }

                    selectimageRect = obj?.GetComponent<RectTransform>();
                }
                return selectimageRect;
            }
        }

        private void Awake()
        {
            _Init();
        }

        public void _Init()
        {
            if (SelectimageRect != null)
            {
                var image = SelectimageRect.GetComponent<Image>();
                if (image != null)
                {
                    image.material = selectimageMaterial;
                    image.sprite = selectimageSprite;
                    image.color = selectimageColor;
                }
                KeepLayer();
            }
        }

        public void KeepLayer()
        {
            SlotHandler?.KeepGridCenter(SelectimageRect, selectimageSize);

            SelectimageRect.gameObject.SetActive(SlotHandler.IsSelected);

            if (SlotHandler.IsSelected && SlotHandler.IsLocked == false)
                SelectimageRect.SetSiblingIndex(layer);
        }
    }

}