using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishTools;

/// <summary>
/// 静态UI管理器,限于当前加载的场景
/// </summary>

namespace EasyUI
{
    public class UIManager_static : BaseSingletonMono<UIManager_static>
    {
        public DictionarySerializable<string, GameObject> panelDict = new DictionarySerializable<string, GameObject>();

        protected override void Awake()
        {
            base.Awake();
            BaseStaticUI[] allPanels = FindObjectsOfType<BaseStaticUI>(true);
            foreach (var panel in allPanels)
            {
                AddPanel(panel.panelname, panel.gameObject);
            }
        }

        internal void AddPanel(string panelName, GameObject panel)
        {
            if (panelDict.ContainsKey(panelName))
            {
#if UNITY_EDITOR
                Debug.LogError($"面板名字被占用了{panel}");
#endif
            }
            else
            {
                panelDict.Add(panelName, panel);
            }
        }

        internal void RemovePanel(string panelName)
        {
            if (panelDict.Remove(panelName))
            {
#if UNITY_EDITOR
                Debug.Log($"移除面板成功:{panelName}");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"面板不存在:{panelName}");
#endif
            }
        }

        // 封装的获取面板方法，复用逻辑
        private GameObject GetPanel(string panelName)
        {
            panelDict.TryGetValue(panelName, out var p);
            if (p == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"未找到面板{panelName}");
#endif
            }
            return p;
        }


        public GameObject OpenPanel(string panelName)
        {
            var p = GetPanel(panelName);
            if (p == null) return null;

            if (p.activeSelf == false)
            {
                p.SetActive(true);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"面板已经打开:{p.name}");
#endif
            }
            return p;
        }

        public GameObject ClosePanel(string panelName)
        {
            var p = GetPanel(panelName);
            if (p == null) return null;

            if (p.activeSelf == true)
            {
                p.SetActive(false);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"面板已经关闭:{p.name}");
#endif
            }
            return p;
        }

        public GameObject RepeatPanel(string panelName)
        {
            var p = GetPanel(panelName);
            if (p == null) return null;

            if (p.activeSelf == true)
            {
                p.SetActive(false);
            }
            else
            {
                p.SetActive(true);
            }
            return p;

        }
    }
}
