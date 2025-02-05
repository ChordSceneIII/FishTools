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
        [Label("当前面板")] public LayerPanel currentpanel;
        [Label("当前选择"), SerializeField, ReadOnly] private GameObject selectobj;

        public Selectable currentSelectable
        {
            get
            {
                selectobj = EventSystem.current.currentSelectedGameObject;
                if (selectobj != null)
                    return selectobj.GetComponent<Selectable>();
                else
                    return null;
            }
        }
        public static LayerInputModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = EventSystem.current.GetComponent<LayerInputModule>();
                }

                return instance;
            }
        }
        private static LayerInputModule instance;

        private void Update()
        {
            if (currentpanel != null && selectobj == null)
            {
                //失去选择目标时自动修复
                if (Input.GetButtonDown(horizontalAxis) || Input.GetButtonDown(verticalAxis))
                {
                    currentpanel.FirstSelect?.Select();
                }
            }
        }

        public override void Process()
        {
            base.Process();

            if (Instance == null)
            {
                return;
            }

            //自动修正currentpanel
            if (currentpanel == null || currentpanel.gameObject.activeInHierarchy == false)
            {
                currentpanel = FindObjectOfType<LayerPanel>(false);
                currentpanel?.Enter();
            }

            if (currentpanel != null)
            {
                //修改首选项
                if (!currentpanel.keepLastSelect && currentSelectable?.transform.IsChildOf(currentpanel.transform) == true)
                {
                    currentpanel.FirstSelect = currentSelectable;
                }

                if (Input.GetButtonDown(cancelButton))
                {
                    currentpanel.OnCancelEvent();
                }
            }

        }

    }


}

