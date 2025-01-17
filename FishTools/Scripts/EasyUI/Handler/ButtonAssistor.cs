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
    public class ButtonAssistor : MonoBehaviour
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
            try
            {
                if (Btn != null && Btn.interactable)
                {
                    if (!isKey && Input.GetButtonDown(button) || isKey && Input.GetKeyDown(key))
                    {
                        // 模拟按钮按下状态
                        Btn.OnPointerDown(new PointerEventData(EventSystem.current));
                    }
                    else if (!isKey && Input.GetButtonUp(button) || isKey && Input.GetKeyUp(key))
                    {
                        // 触发点击事件
                        Btn.onClick.Invoke();

                        // 模拟按钮抬起状态
                        Btn.OnPointerUp(new PointerEventData(EventSystem.current));
                    }
                }
            }
            catch { }
        }

        //TODO 添加按下，按住和松开的事件
        //TODO  鼠标自动移动区域设置
        //TODO  按钮就只需要按下时显示，离开时松开即可，固定按键

        //TODO 做下对characterEntity的替换SceneUI
        //TODO 屏幕移动做完，做物品的锁定
    }
}
