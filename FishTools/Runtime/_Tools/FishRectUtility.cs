using System.Collections.Generic;
using UnityEngine;

namespace FishTools
{
    public static class FishRectUtility
    {
        /// <summary>
        /// <para>@判断两个Rect物体的相对位置关系</para>
        /// <para>(-1,-1)代表在左下,(1,0)代表在右边,(0,0)代表在内部。overflow=true表示判断物体边界超出</para>
        /// </summary>
        public static Vector2 DecideRectDirection(RectTransform child, RectTransform parent, bool overflow = true)
        {
            Vector2 direction = Vector2.zero;

            Vector2 childMin = child.TransformPoint(child.rect.min);
            Vector2 childMax = child.TransformPoint(child.rect.max);
            Vector2 parentMin = parent.TransformPoint(parent.rect.min);
            Vector2 parentMax = parent.TransformPoint(parent.rect.max);

            if (childMax.x < parentMin.x)
            {
                direction.x = -1;
            }
            else if (overflow && childMin.x < parentMin.x)
            {
                direction.x = -1;
            }

            if (childMin.x > parentMax.x)
            {
                direction.x = 1;
            }
            else if (overflow && childMax.x > parentMax.x)
            {
                direction.x = 1;
            }

            if (childMax.y < parentMin.y)
            {
                direction.y = -1;
            }
            else if (overflow && childMin.y < parentMin.y)
            {
                direction.y = -1;
            }

            if (childMin.y > parentMax.y)
            {
                direction.y = 1;
            }
            else if (overflow && childMax.y > parentMax.y)
            {
                direction.y = 1;
            }

            return direction;
        }

    }
}
