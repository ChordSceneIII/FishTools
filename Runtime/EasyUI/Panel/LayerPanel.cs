using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FishTools.EasyUI
{
    /// <summary>
    /// <para>顺序面板，可以自动切换聚焦目标</para>
    /// <para>若要嵌套使用，请把Group的ignoreParent勾选</para>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class LayerPanel : BasePanel
    {
        [SerializeField, ReadOnly] private CanvasGroup _group;
        public CanvasGroup Group => FishUtility.LazyGet(this, ref _group);
        [SerializeField] private bool cancelClose = true;
        [Label("保持首选项")] public bool keepLastSelect;
        [Label("首选项")] public Selectable firstSelect;
        [Label("默认取消"),Tooltip("是否使用全局配置的取消按钮")] public bool useDefaultCancel = true;
        [ConField("useDefaultCancel", false)] public string cancelButton = "Cancel";
        [SerializeField, ReadOnly] private LayerPanel _lastpanel = null;
        public UnityEvent onCancelEvent = new();

        private LayerInputModule Module => LayerInputModule.Instance;
        public Selectable FirstSelect
        {
            get
            {
                if (FishUtility.IsNull(firstSelect) || firstSelect.interactable == false)
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
            Focus();
        }
        private void OnDisable()
        {
            if (gameObject.scene.isLoaded == false) return;

            Module.LayerChains.Remove(this);

            Module.LayerChains.LastOrDefault()?.Focus();

        }

        public void Cancel()
        {
            onCancelEvent?.Invoke();

            Group.interactable = false;

            if (cancelClose)
                Close();

        }

        public void Focus()
        {
            _lastpanel = Module.LayerChains.LastOrDefault();

            if (_lastpanel != null)
                _lastpanel.Group.interactable = false;

            Module.LayerChains.Remove(this);
            Module.LayerChains.Add(this);

            Group.interactable = true;
            FirstSelect?.Select();

            transform.SetAsLastSibling();

            Module.currentpanel = this;
        }

    }
}
