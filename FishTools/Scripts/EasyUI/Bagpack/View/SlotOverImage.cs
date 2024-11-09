using UnityEngine;
using UnityEngine.UI;
using FishTools;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(SlotGroup))]
    public class SlotOverImage : View
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

        [SerializeField, ReadOnly] private RectTransform overimageRect;
        [Label("尺寸")] public Vector2 overimageSize = new Vector2(1.0f, 1.0f);
        [Label("图片")] public Sprite overimageSprite;
        [Label("材质")] public Material overimageMaterial;
        [Label("颜色")] public Color overimageColor = Color.white;
        [Label("层级")] public int layer = 0;


        public RectTransform OverimageRect
        {
            get
            {
                if (overimageRect != null && overimageRect.parent != this.transform)
                {
                    DestroyImmediate(overimageRect.gameObject);
                }

                if (overimageRect == null)
                {
                    var obj = transform.Find("overimage")?.gameObject;

                    if (obj == null)
                    {
                        obj = ViewUtils.CreateNewImage("overimage", transform, overimageSprite, overimageColor, overimageSize, overimageMaterial);
                    }

                    overimageRect = obj?.GetComponent<RectTransform>();
                }
                return overimageRect;
            }
        }

        //TODO:加一个 范例模板，加一个Text组件

        private void Awake()
        {
            _Init();
        }

        public void _Init()
        {
            var image = OverimageRect.GetComponent<Image>();
            if (image != null)
            {
                image.material = overimageMaterial;
                image.sprite = overimageSprite;
                image.color = overimageColor;
            }
            KeepLayer();
        }

        public void KeepLayer()
        {
            SlotHandler?.KeepGridCenter(OverimageRect, overimageSize);
            overimageRect.gameObject.SetActive(SlotHandler.IsLocked);
            if (SlotHandler.IsLocked)
            {
                overimageRect.gameObject.SetActive(SlotHandler.IsLocked);
                OverimageRect.SetSiblingIndex(layer);
            }

        }
    }


}