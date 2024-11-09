using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 动态UI管理器
/// </summary>

namespace FishTools.EasyUI
{
    public class UIManager_develop : BaseSingletonMono<UIManager_develop>
    {
        //资源路径
        internal Dictionary<string, string> pathDict = new Dictionary<string, string>();

        //预制件缓存
        internal Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

        //已打开界面缓存引用
        internal Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);

            LoadPath();
        }
        //加载资源路径
        internal static void LoadPath()
        {
            var script_objs = Resources.LoadAll<ScriptableObject>("");

            var datas = script_objs.OfType<IBasePanelData>().ToList();

            foreach (var data in datas)
            {
                data?.AddToPathDict();
            }
        }

        //获取预制件并加入到预制件缓存中
        public bool TryGetPrefab(string name, out GameObject prefab)
        {
            prefab = null;
            //检查路径缓存
            if (!pathDict.TryGetValue(name, out var path))
            {
                DebugF.LogError("界面名称错误，或未配置路径:" + name);
                return false;
            }

            //加载资源到内存中
            if (!prefabDict.TryGetValue(name, out var panelPrefab))
            {
                try
                {
                    /// <summary>
                    /// 路径读取关键：{path}位置于PanelData中自动获取,前提是必须是Resources目录下
                    /// </summary>
                    panelPrefab = Resources.Load<GameObject>(path);

                    if (panelPrefab == null)
                    {
                        DebugF.LogError("无法加载预制件,请检查路径是否正确:" + path);
                        return false;
                    }

                    //把预制件添加到缓存中，方便下一次调用不需要重复使用Resources.Load 读盘
                    prefabDict.Add(name, panelPrefab);
                }
                catch (System.Exception ex)
                {
                    DebugF.LogError("加载预制件失败: " + path + "\n" + ex.Message);
                    return false;
                }
            }

            prefab = panelPrefab;
            return prefab != null;
        }

        //获取已经存在的面板
        public BasePanel GetExistPanel(string name)
        {
            panelDict.TryGetValue(name, out var panel);
            return panel;
        }

        //打开界面
        public BasePanel OpenPanel(string name)
        {
            //如果存在面板则返回panel，否则创建新的panel
            if (panelDict.TryGetValue(name, out var panel)) { }
            else { panel = GetNewPanel(name); }


            panel.gameObject.SetActive(true);

            return panel;

        }

        //关闭界面
        public BasePanel ClosePanel(string name)
        {
            if (panelDict.TryGetValue(name, out var panel)) { }
            else { panel = GetNewPanel(name); }


            panel.gameObject.SetActive(false);
            return panel;
        }

        //面板开关，打开关闭
        public BasePanel RepeatPanel(string name)
        {
            if (panelDict.TryGetValue(name, out var panel)) { }
            else { panel = GetNewPanel(name); }

            panel.gameObject.SetActive(!panel.gameObject.activeSelf);
            return panel;
        }

        //面板开关，移除和生成
        public BasePanel RepeatNewPanel(string name)
        {
            //如果界面显示了
            if (panelDict.TryGetValue(name, out var panel))
            {
                DestroyPanel(name);
            }
            else
            {
                panel = GetNewPanel(name);
            }

            return panel;
        }

        //生成面板并加入到缓存
        public BasePanel GetNewPanel(string name, bool isActive = true)
        {
            if (panelDict.ContainsKey(name))
            {
                DebugF.LogError("面板已经存在:" + name);
                return null;
            }

            if (TryGetPrefab(name, out var panelPrefab))
            {

                GameObject panelObject = GameObject.Instantiate(panelPrefab, panelPrefab.GetComponent<BasePanel>().UIRoot, false);

                var panel = panelObject?.GetComponent<BasePanel>();


                if (panel == null)
                {
                    DebugF.LogError($"预制件上没有找到 {panel} 组件:" + panelObject.name);
                    Destroy(panelObject);
                    return null;
                }


                //加入到缓存路径，表示已经显示的界面
                panelDict.Add(name, panel);
                panelObject.SetActive(isActive);

                DebugF.Log($"从预制体中加载了一个面板{panelObject}");
                return panel;
            }
            else
            {
                return null;
            }
        }

        //移除面板并移除缓存
        public bool DestroyPanel(string name)
        {
            // 检查是否打开并获取面板
            if (panelDict.TryGetValue(name, out var panel))
            {
                // 移除面板，表示面板不存在
                panelDict.Remove(name);
                // 关闭界面
                panel.gameObject.SetActive(false);
                Destroy(panel.gameObject);
                return true;
            }
            return false;
        }

    }
}
