using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    /// <summary>
    /// Button 自定义按键
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonAssistor : BaseHandler
    {
        private Button btn;
        public Button Btn => FishUtility.LazyGet(this, ref btn);
        [Label("Key/Button")] public bool isKey = true;
        [ConField("isKey", false)] public string button;
        [ConField("isKey", true)] public KeyCode key;
        [Label("长按")] public bool isLongPress;
        [Label("长按动画"), ConField("isLongPress", true)] public AnimationClip clip;

        protected override void Update()
        {
            base.Update();
            // 监听输入
            if (interactable && Btn.interactable)
            {
                if (!isKey && Input.GetButtonDown(button) || isKey && Input.GetKeyDown(key))
                {


                    // 模拟按钮按下状态
                    Btn.OnPointerDown(new PointerEventData(EventSystem.current));
                }
                else if (!isKey && Input.GetButtonUp(button) || isKey && Input.GetKeyUp(key))
                {

                    // 触发Submit事件
                    ExecuteEvents.Execute(Btn.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);


                    // 模拟按钮抬起状态
                    Btn.OnPointerUp(new PointerEventData(EventSystem.current));
                }
            }

        }
    }
}
