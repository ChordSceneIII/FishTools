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
        public Button Btn
        {
            get
            {
                if (btn == null) btn = GetComponent<Button>();
                return btn;
            }
        }
        [Label("Key/Button")] public bool isKey = true;
        [ConditionalField("isKey", false)] public string button;
        [ConditionalField("isKey", true)] public KeyCode key;
        [Label("长按")] public bool isLongPress;
        [Label("长按动画"), ConditionalField("isLongPress", true)] public AnimationClip clip;

        private void Update()
        {
            // 监听输入
            if (IsInteractable() && Btn.interactable)
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
