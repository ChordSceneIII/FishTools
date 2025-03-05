using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//增设ISelectHandler 用于

namespace FishTools.EasyUI
{

    [Icon("d_StandaloneInputModule Icon")]
    [RequireComponent(typeof(EventSystem))]
    public class LayerInputModule : StandaloneInputModule
    {
        [Label("根面板(菜单)")] public LayerPanel _rootPanel;
        [Label("当前面板"), SerializeField, ReadOnly] private LayerPanel _currentpanel;
        public LayerPanel currentpanel
        {
            get
            {
                if (_currentpanel == null)
                {
                    _currentpanel = layerChains.FirstOrDefault();
                }
                return _currentpanel;
            }
            set => _currentpanel = value;
        }

        [Label("当前选择"), SerializeField, ReadOnly] private GameObject _selectobj;
        public Selectable currentSelectable
        {
            get
            {
                _selectobj = EventSystem.current.currentSelectedGameObject;
                if (_selectobj != null)
                    return _selectobj.GetComponent<Selectable>();
                else
                    return null;
            }
        }

        [SerializeField, ReadOnly] private List<LayerPanel> layerChains = new List<LayerPanel>();
        public List<LayerPanel> LayerChains => layerChains;

        private static LayerInputModule _instance;
        public static LayerInputModule Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = EventSystem.current.GetComponent<LayerInputModule>();
                }

                return _instance;
            }
        }

        public override void Process()
        {
            base.Process();

            if (Instance == null)
            {
                return;
            }

            if (currentpanel != null && _selectobj == null)
            {
                //失去选择目标时自动修复
                if (Input.GetButtonDown(horizontalAxis) || Input.GetButtonDown(verticalAxis))
                {
                    currentpanel.FirstSelect?.Select();
                }
            }

            //修改首选项
            if (currentpanel != null && !currentpanel.keepLastSelect && currentSelectable?.transform.IsChildOf(currentpanel.transform) == true)
            {
                currentpanel.FirstSelect = currentSelectable;
            }

            //没有面板时打开根面板
            if (LayerChains.Count==0)
            {
                _rootPanel.Open();
            }

            //使用自定义的取消键
            if (currentpanel.useDefaultCancel == false && Input.GetButtonDown(currentpanel.cancelButton))
            {
                if (currentpanel == _rootPanel && layerChains.Count <= 1)
                {
                    currentpanel.onCancelEvent?.Invoke();
                    return;
                }

                currentpanel.Cancel();
            }

            //使用默认的取消键
            if (currentpanel.useDefaultCancel == true && Input.GetButtonDown(cancelButton))
            {
                if (currentpanel == _rootPanel && layerChains.Count <= 1)
                {
                    currentpanel.onCancelEvent?.Invoke();
                    return;
                }
                currentpanel.Cancel();
            }

        }
    }
}

