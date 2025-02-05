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
        [Label("取消事件")] public UnityEvent cancelEvent;

        private CanvasGroup group;
        public CanvasGroup Group => group ??= GetComponent<CanvasGroup>();
        private LayerInputModule LayerModule => LayerInputModule.Instance;
        public Selectable FirstSelect
        {
            get
            {
                if (FishUtility.IsNull(firstSelect))
                {
                    foreach (var selectable in GetComponentsInChildren<Selectable>())
                    {
                        if (selectable.interactable && selectable.navigation.mode != Navigation.Mode.None)
                        {
                            firstSelect = selectable;
                            return firstSelect;
                        }
                    }
                    return null;
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

        internal void Enter()
        {
            if (!keepLastPanel && LayerModule.currentpanel != this)
                lastPanel = LayerModule.currentpanel;

            lastPanel?.UnFocus();
            Focus();
        }

        internal void Exit()
        {

            if (cancelFocus)
                UnFocus();

            lastPanel?.Focus();
        }

        internal void OnCancelEvent()
        {
            Exit();

            if (cancelClose)
                Close();

            cancelEvent?.Invoke();
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
