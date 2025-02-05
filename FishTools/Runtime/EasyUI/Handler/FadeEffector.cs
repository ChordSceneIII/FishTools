using System.Collections;
using System.Linq;
using FishTools.TimeTool;
using UnityEngine;

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
        public void IsVisible(bool value) { isVisible = value; }
        [Label("渐变曲线(进入)")] public AnimationCurve curveOn = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);
        [Label("渐变曲线(退出)")] public AnimationCurve curveOff = AnimationCurve.Linear(0, 0, 0.5f, 1);

        private bool lastIsvisible;
        public FTimer timer = new FTimer();
        //TODO:这里的效果改成使用不受Time.Scale影响的

        CanvasGroup group;
        public CanvasGroup Group
        {
            get
            {
                if (group == null) group = GetComponent<CanvasGroup>();
                return group;
            }
        }

        private void OnEnable()
        {
            if (Group == null) return;

            Group.alpha = 0;
            timer.Register(1f).Start().OnUpdate(() =>
            {
                if (isVisible)
                    Group.alpha = curveOn.Evaluate(timer.GetForwardTime());
                else
                    Group.alpha = curveOff.Evaluate(timer.GetResultTime());
            }); ;
        }

        private void OnDisable()
        {
            timer.UnRegister();
        }

        void Update()
        {
            if (lastIsvisible != isVisible)
            {
                lastIsvisible = isVisible;
                timer.ReStart();
            }
        }
    }
}