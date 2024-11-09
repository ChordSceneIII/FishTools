using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 界面池管理，与UIManager_develop不同的是，pool是可以生成多个实例的，而develop是一个预制体对应一个实例
/// </summary>

namespace FishTools.EasyUI
{
    public class UIManager_pool : BaseSingletonMono<UIManager_pool>
    {
        //UI池
        [SerializeField] internal DictionarySerializable<string, BasePanel> panelPool = new DictionarySerializable<string, BasePanel>();

        protected override void Awake()
        {
            base.Awake();
        }

        //清理无效引用
        private void CleanNullReferences()
        {
            var keysToRemove = new List<string>();
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

        public bool ContainsPanel(string panelIndex)
        {
            CleanNullReferences();
            return panelPool.ContainsKey(panelIndex);
        }
        public bool TryGetPanel(string panelIndex, out BasePanel panel)
        {
            CleanNullReferences();
            panelPool.TryGetValue(panelIndex, out BasePanel _panel);
            panel = _panel;
            return panel != null;
        }

        //从预制体资源生成一个新的Panel
        public BasePanel GetNewPanel(string panelname, string panelIndex, bool isActive = true)
        {
            //每次注册对象池时先清理无效引用，减少GC
            CleanNullReferences();
            if (panelPool.ContainsKey(panelIndex))
            {
                DebugF.LogError($"对象池中已经存在标记为 {panelIndex} 的面板");
                return null;
            }

            if (UIManager_develop.Instance.TryGetPrefab(panelname, out var panelPrefab))
            {
                GameObject panelObject = GameObject.Instantiate(panelPrefab, panelPrefab.GetComponent<BasePanel>().UIRoot, false);

                var panel = panelObject?.GetComponent<BasePanel>();


                if (panel == null)
                {
                    DebugF.LogError($"预制件上没有找到 {panel} 组件:" + panelObject.name);
                    Destroy(panelObject);
                    return null;
                }

                //加入到UI池中
                panelPool.Add(panelIndex, panel);

                panelObject.SetActive(isActive);

                DebugF.Log($"加载了一个新的面板{panelname},标记为:{panelIndex}");
                return panel;
            }
            else
            {
                return null;
            }
        }

        public bool DestoryPanel(string panelIndex)
        {
            if (panelPool.TryGetValue(panelIndex, out var panel))
            {
                panel.gameObject.SetActive(false);
                Destroy(panel.gameObject);
                return true;
            }
            else
            {
                DebugF.LogWarning($"通过{panelIndex}未找到面板");
                return false;
            }

        }

        public BasePanel OpenPanel(string panelIndex)
        {
            if (panelPool.TryGetValue(panelIndex, out var panel))
            {
                panel.gameObject.SetActive(true);
                return panel;
            }
            else
            {
                DebugF.LogWarning($"通过{panelIndex}未找到面板");
                return default;
            }
        }

        public BasePanel RepatPanel(string panelIndex)
        {
            if (panelPool.TryGetValue(panelIndex, out var panel))
            {
                panel.gameObject.SetActive(!panel.gameObject.activeSelf);
                return panel;
            }
            else
            {
                DebugF.LogWarning($"通过{panelIndex}未找到面板");
                return default;
            }
        }

        public BasePanel ClosePanel(string panelIndex)
        {
            if (panelPool.TryGetValue(panelIndex, out var panel))
            {
                panel.gameObject.SetActive(false);
                return panel;
            }
            else
            {
                DebugF.LogWarning($"通过{panelIndex}未找到面板");
                return default;
            }
        }
    }
}
