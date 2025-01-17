using FishTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    /// <summary>
    /// ScrollRect 根据子项选择自动调整滚动位置
    /// </summary>
    [Icon("StandaloneInputModule Icon")]
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScroll : MonoBehaviour
    {
        [ReadOnly, SerializeField] private ScrollRect scrollRect;
        public ScrollRect ScrollRect
        {
            get
            {
                if (scrollRect == null)
                {
                    scrollRect = GetComponent<ScrollRect>();
                }
                return scrollRect;
            }
        }
        [Range(0, 1)] public float rateX = 0.5f;
        [Range(0, 1)] public float rateY = 0.5f;

        private void Update()
        {
            AutoFixedScrollBar();
        }

        /// <summary>
        /// 自动调整位置
        /// </summary>
        void AutoFixedScrollBar()
        {
            // 获取当前选中的 UI 对象
            GameObject selected = EventSystem.current?.currentSelectedGameObject;

            // 确保选中的对象在 ScrollRect 的Content下
            if (selected != null && selected.transform.IsChildOf(ScrollRect.content))
            {
                var selectedRectTransform = selected.transform as RectTransform;
                var scrollRectTransform = ScrollRect.transform as RectTransform;

                //获取相对位置
                var direction = FishUtility.CompareRectDirection(selectedRectTransform, scrollRectTransform, true);

                //不在对象内部时，对滚动条value进行调整直到完全被显示
                if (direction != new Vector2(0, 0))
                {
                    float rectXDelta = selectedRectTransform.rect.width / ScrollRect.content.rect.width / 4;
                    float rectYDelta = selectedRectTransform.rect.height / ScrollRect.content.rect.height / 4;

                    var horiziontalbar = ScrollRect.horizontalScrollbar;
                    var verticalbar = ScrollRect.verticalScrollbar;

                    if (horiziontalbar != null)
                    {
                        horiziontalbar.value += rateX * direction.x * rectXDelta;
                    }
                    if (verticalbar != null)
                    {
                        verticalbar.value += rateY * direction.y * rectYDelta;
                    }
                }
            }
        }
    }
}