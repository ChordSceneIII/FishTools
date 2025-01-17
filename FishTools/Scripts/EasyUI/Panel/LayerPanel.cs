using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 顺序面板，可以自动切换聚焦目标
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class LayerPanel : BasePanel
    {
        [Label("取消时失去焦点")] public bool cancelFocus = true;
        [Label("取消时关闭面板")] public bool cancelClose = true;
        [Label("保持首选项")] public bool keepLastSelect;
        [Label("首选项")] public Selectable firstSelect;
        [Label("保持上级面板")] public bool keepLastPanel;
        [Label("上级面板")] public LayerPanel lastPanel;
        [Label("进入事件")] public UnityEvent enterEvent;
        [Label("取消事件")] public UnityEvent exitEvent;

        private CanvasGroup group;
        public CanvasGroup Group => group ??= GetComponent<CanvasGroup>();
        private LayerInputModule LayerModule => LayerInputModule.Instance;
        public Selectable FirstSelect
        {
            get
            {
                if (firstSelect == null)
                {
                    foreach (var selectable in GetComponentsInChildren<Selectable>())
                    {
                        if (selectable.interactable && selectable.navigation.mode != Navigation.Mode.None)
                        {
                            firstSelect = selectable;
                            break;
                        }
                    }
                    firstSelect = null;
                }
                return firstSelect;
            }
            set
            {
                firstSelect = value;
            }
        }


        private void OnEnable()
        {
            Enter();
        }
        private void OnDisable()
        {
            if (gameObject.scene.isLoaded == false) return;
            Exit();
        }

        public void Enter()
        {
            enterEvent?.Invoke();

            if (!keepLastPanel && LayerModule.currentpanel != this)
                lastPanel = LayerModule.currentpanel;

            lastPanel?.UnFocus();
            Focus();
        }

        public void Exit()
        {
            exitEvent?.Invoke();

            if (cancelFocus)
                UnFocus();

            lastPanel?.Focus();

        }

        public void OnCancelEvent()
        {
            Exit();

            if (cancelClose)
                Close();
        }

        private void Focus()
        {
            if (gameObject.activeInHierarchy)
            {
                Group.interactable = true;

                FirstSelect?.Select();

                transform.SetAsLastSibling();

                LayerModule.currentpanel = this;
            }
        }
        private void UnFocus()
        {
            Group.interactable = false;
        }
    }
}
