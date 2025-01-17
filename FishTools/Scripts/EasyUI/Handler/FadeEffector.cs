using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 渐变效果
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeEffector : MonoBehaviour
    {
        //子对象选中时激活
        [Label("显示")] public bool isVisible;
        [Label("渐变曲线")] public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);
        private float timer = 0f; // 计时器

        CanvasGroup group;
        public CanvasGroup Group
        {
            get
            {
                if (group == null) group = GetComponent<CanvasGroup>();
                return group;
            }
        }

        protected void OnEnable()
        {
            Group.alpha = 0f;
            timer = 0f;
        }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        void Update()
        {
            UpdateVisible();
        }

        //TODO 优化处理

        private void UpdateVisible()
        {
            // 根据鼠标状态更新计时器
            if (isVisible)
            {
                timer += Time.deltaTime; // 鼠标进入时，计时器增加
            }
            else
            {
                timer -= Time.deltaTime; // 鼠标离开时，计时器减少
            }

            // 限制计时器范围在 [0, 1]
            timer = Mathf.Clamp01(timer);

            // 根据曲线设置 alpha
            Group.alpha = curve.Evaluate(timer);
        }
    }
}