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
        public ScrollRect ScrollRect => FishUtility.LazyGet(this, ref scrollRect);
        public float delayTime = 0.1f;
        public float extendTime = 0.3f;
        public float speed = 5f;
        [Label("能否被打断")] public bool canBreak = false;
        [SerializeField, ReadOnly] private GameObject lastSelect = null;

        private void Update()
        {
            // 获取当前选中的 UI 对象
            GameObject selected = EventSystem.current?.currentSelectedGameObject;

            // 确保选中的对象在 ScrollRect 的Content下
            if (lastSelect != selected)
            {
                lastSelect = selected;

                if (lastSelect == null) return;

                Locate(lastSelect);
            }
        }

        /// <summary>
        /// 自动调整位置
        /// </summary>
        public void Locate(GameObject selected)
        {
            if (selected.transform.IsChildOf(ScrollRect.content) == false) return;

            var selRec = selected.transform as RectTransform;
            var scrollRec = ScrollRect.transform as RectTransform;

            //获取相对位置
            var direction = FishRectUtility.DecideRectDirection(selRec, scrollRec);

            //不在对象内部时，对滚动条value进行调整直到完全被显示
            if (direction != new Vector2(0, 0))
            {
                var delta = selected.transform.position - ScrollRect.content.TransformPoint(ScrollRect.content.rect.min);

                FMonitor.Create("AutoScroll_Running").RealTime(true).Delay(delayTime).Condition(() =>
                {
                    if ((Input.anyKey && canBreak)
                    || FishRectUtility.DecideRectDirection(selRec, scrollRec) == new Vector2(0, 0))
                    {
                        return true;
                    }
                    else return false;
                })
                .Extend(extendTime, true)
                .OnUpdate(() =>
                    {
                        ScrollRect.horizontalNormalizedPosition += direction.x * speed * Time.unscaledDeltaTime;
                        ScrollRect.verticalNormalizedPosition += direction.y * speed * Time.unscaledDeltaTime;
                    }
                );
            }
        }
    }
}
