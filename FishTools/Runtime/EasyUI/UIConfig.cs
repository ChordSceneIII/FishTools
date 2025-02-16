using System;
using UnityEngine;

/// <summary>
/// 动态UI管理器
/// </summary>

namespace FishTools.EasyUI
{
    [Serializable]
    public struct PanelData
    {
        [Label("面板")] public BasePanel basepanel;
        [Label("根画布")] public string rootName;
        public GameObject gameObject => basepanel?.gameObject;
        private Transform uiroot;
        public Transform UIRoot
        {
            get
            {
                if (uiroot == null || (uiroot != null && uiroot.gameObject.name != rootName))
                {
                    uiroot = FishUtility.FindComponent<RectTransform>(rootName, true)?.transform;
                    if (uiroot == null)
                    {
                        uiroot = GameObject.FindObjectOfType<Canvas>(false).transform;
                        DebugF.LogWarning($"找不到指定的RectTransform:{rootName},自动寻找可用Canvas");
                    }
                }
                return uiroot;
            }
        }
    }

    [CreateAssetMenu(fileName = "uiconfig", menuName = "FishTools/EayUI/Config", order = 1)]
    public class UIConfig : ScriptableObject
    {
        //路径配置 (索引，数据)
        public DictionarySer<string, PanelData> path = new DictionarySer<string, PanelData>();

        //已加载页面缓存
        [SerializeField, Label("页面缓存")] internal DictionarySer<string, BasePanel> panel_cache = new DictionarySer<string, BasePanel>();

        /// <summary>
        /// 从缓存中获取面板
        /// </summary>
        public BasePanel GetCachePanel(string name)
        {
            panel_cache.ClearNullValues();
            if (!panel_cache.TryGetValue(name, out var panel))
            {
                return null;
            }
            return panel;
        }

        /// <summary>
        /// 判断面板是否已经存在
        /// </summary>
        public bool ContainsPanel(string name)
        {
            panel_cache.ClearNullValues();
            return panel_cache.ContainsKey(name);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        public void OpenPanel(string name)
        {
            ForcePanel(name, name, true);
        }
        /// <summary>
        /// 关闭界面
        /// </summary>
        public void ClosePanel(string name)
        {
            ForcePanel(name, name, false);
        }

        /// <summary>
        /// 面板开关重复
        /// </summary>
        public void RepeatPanel(string name)
        {
            GetCachePanel(name).Repeat();
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        public void DestroyPanel(string name)
        {
            panel_cache.ClearNullValues();
            // 检查是否打开并获取面板
            if (panel_cache.TryGetValue(name, out var panel))
            {
                // 移除面板，表示面板已经移除
                panel_cache.Remove(name);
                // 关闭界面
                panel.gameObject.SetActive(false);
                Destroy(panel.gameObject);
            }
        }

        /// <summary>
        /// 强制执行Panel，无论是否存在
        /// </summary>
        public BasePanel ForcePanel(string key, string name, bool isActive = true)
        {
            panel_cache.ClearNullValues();
            if (panel_cache.TryGetValue(name, out var panel))
            {
                if (isActive)
                    panel.Open();
                else
                    panel.Close();
                return panel;
            }
            else
            {
                return GetNewPanel(key, name, isActive);
            }
        }

        /// <summary>
        /// 生成新面板并加入到缓存
        /// </summary>
        public BasePanel GetNewPanel(string key, string name, bool isActive = true)
        {
            //检查索引是否不存在
            if (!path.ContainsKey(key))
            {
                DebugF.LogWarning($"key[{key}]面板不存在");
                return null;
            }

            panel_cache.ClearNullValues();

            //检查面板是否已经加载
            if (panel_cache.ContainsKey(name))
            {
                DebugF.LogWarning($"面板已经存在: {name}, cache: {panel_cache[name]} ");
                return null;
            }

            if (path[key].basepanel == null)
            {
                DebugF.LogWarning($"找不到指定面板: {key}，请检查配置文件 {this}");
                return null;
            }

            GameObject obj = GameObject.Instantiate(path[key].gameObject, path[key].UIRoot);
            obj.name = name;
            var panel = obj.GetComponent<BasePanel>();

            //加入到缓存路径，表示已经加载的面板
            panel_cache.Add(name, panel);
            obj.SetActive(isActive);

            DebugF.Log($"从预制体中加载了一个面板{obj}");
            return panel;
        }

        /// <summary>
        /// 清理所有面板以及缓存垃圾
        /// </summary>
        public void ClearAllPanels()
        {
            panel_cache.ClearNullValues();
            foreach (var panel in panel_cache.Values)
            {
                if (panel != null)
                {
                    if (Application.isPlaying)
                        Destroy(panel.gameObject);
                    else
                        DestroyImmediate(panel.gameObject);
                }
            }
            panel_cache.Clear();
            // DebugF.Log("页面缓存已清除");
        }

        //TODO：可以单独把缓存放在单例中，ScriptObejct访问缓存就访问单例，然后单例可以对缓存内容访问(打开和关闭，不负责创建和销毁)

    }
}
