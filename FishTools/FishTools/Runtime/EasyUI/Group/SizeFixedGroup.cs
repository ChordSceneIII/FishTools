using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{

    public class SizeFixedGroup : LayoutGroup
    {
        public bool fixedHorizontal = true;
        public bool fixedVertical = true;

        [Header("状态参数 (transform.position)")]
        [ReadOnly] public Vector2 minPos;
        [ReadOnly] public Vector2 maxPos;
        [ReadOnly] public Vector2 virtualCenter;

        public override void CalculateLayoutInputVertical()
        {
            UpdateVirtualSize();
        }

        public override void SetLayoutHorizontal()
        {
            UpdateHorizontal();
        }

        public override void SetLayoutVertical()
        {
            UpdateVertical();
        }
        private void UpdateHorizontal()
        {
            if (fixedHorizontal)
            {
                float offsetX = rectTransform.position.x - virtualCenter.x + rectTransform.rect.width * (0.5f - rectTransform.pivot.x);

                if (offsetX != 0)
                {
                    foreach (var child in rectChildren)
                    {
                        child.position += new Vector3(offsetX, 0);
                    }
                }

                rectTransform.sizeDelta = new Vector2(maxPos.x - minPos.x, rectTransform.sizeDelta.y);
            }
        }
        private void UpdateVertical()
        {
            if (fixedVertical)
            {
                float offsetY = rectTransform.position.y - virtualCenter.y + rectTransform.rect.height * (0.5f - rectTransform.pivot.y);

                if (offsetY != 0)
                {
                    foreach (var child in rectChildren)
                    {
                        child.position += new Vector3(0, offsetY);
                    }
                }
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, maxPos.y - minPos.y);
            }
        }

        /// <summary>
        /// 遍历所有子对象的组合矩形的绝对坐标
        /// </summary>
        private void UpdateVirtualSize()
        {
            if (rectChildren.Count == 0)
            {
                return;
            }

            // 以第一个子对象为初始值
            RectTransform first = rectChildren[0];

            float minX = first.position.x - first.rect.width * first.pivot.x;
            float maxX = first.position.x + first.rect.width * (1 - first.pivot.x);
            float maxY = first.position.y + first.rect.height * (1 - first.pivot.y);
            float minY = first.position.y - first.rect.height * first.pivot.y;

            for (int i = 1; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                if (child == null) continue;

                var x = child.position.x - child.rect.width * child.pivot.x;
                var x2 = child.position.x + child.rect.width * (1 - child.pivot.x);
                var y = child.position.y - child.rect.height * child.pivot.y;
                var y2 = child.position.y + child.rect.height * (1 - child.pivot.y);

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (x2 > maxX) maxX = x2;
                if (y2 > maxY) maxY = y2;
            }

            minPos = new Vector2(minX, minY);
            maxPos = new Vector2(maxX + padding.horizontal, maxY + padding.vertical);
            virtualCenter = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        }

    }
}
