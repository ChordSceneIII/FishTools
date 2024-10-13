using System.Collections;
using System.Collections.Generic;
using FishTools;
using FishToolsEditor;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

/// <summary>
/// 界面池管理，与develop不同，采用的是Setactive的方式开关界面，但是是共享一套路径资源
/// </summary>

namespace EasyUI
{
    public class UIManager_pool : BaseSingletonMono<UIManager_pool>
    {
        private Transform _uiRoot;
        public Transform UIRoot
        {
            get
            {
                if (_uiRoot == null || !_uiRoot.gameObject.activeInHierarchy)
                {
                    //设置ui根节点
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                return _uiRoot;
            }
        }

        //UI池
        internal Dictionary<int, BasePanel> panelPool = new Dictionary<int, BasePanel>();

        protected override void Awake()
        {
            base.Awake();
        }

        //清理无效引用
        private void CleanUpNullReferences()
        {
            var keysToRemove = new List<int>();
            foreach (var kvp in panelPool)
            {
                if (kvp.Value == null)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                panelPool.Remove(key);
            }
        }

        public bool isRegistered(int refID)
        {
            return panelPool.ContainsKey(refID);
        }

        //从预制体资源生成一个新的Panel
        public (BasePanel, int) GetNewPanel(string panelname)
        {
            //每次注册对象池时先清理无效引用，减少GC
            CleanUpNullReferences();

            BasePanel panel = null;
            var panelPrefab = UIManager_develop.Instance?.TryGetPrefab(panelname);

            if (panelPrefab != null)
            {
                GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);

                panel = panelObject?.GetComponent<BasePanel>();

                if (panel == null)
                {
                    DebugEditor.LogError("预制件上没有找到 BasePanel 组件:" + name);
                    Destroy(panelObject);
                    return default;
                }

                var refID = panel.GetInstanceID();

                //加入到UI池中
                panelPool.Add(refID, panel);

                DebugEditor.Log($"加载了一个新的面板{panelname},refID:{refID}");
                return (panel, refID);
            }
            else
            {
                return default;
            }
        }

        public BasePanel OpenPanel(int refID)
        {
            if (panelPool.TryGetValue(refID, out var panel))
            {
                panel.gameObject.SetActive(true);
                return panel;
            }
            else
            {
                DebugEditor.LogWarning($"通过{refID}未找到面板");
                return default;
            }
        }

        public (BasePanel, bool) RepatPanel(int refID)
        {
            if (panelPool.TryGetValue(refID, out var panel))
            {
                if (panel.gameObject.activeSelf == false)
                {
                    panel.gameObject.SetActive(true);
                    return (panel, true);
                }
                else
                {
                    panel.gameObject.SetActive(false);
                    return (panel, false);
                }
            }
            else
            {
                DebugEditor.LogWarning($"通过{refID}未找到面板");
                return default;
            }
        }

        public BasePanel ClosePanel(int refID)
        {
            if (panelPool.TryGetValue(refID, out var panel))
            {
                panel.gameObject.SetActive(false);
                return panel;
            }
            else
            {
                DebugEditor.LogWarning($"通过{refID}未找到面板");
                return default;
            }
        }
    }
}
