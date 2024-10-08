using System.Collections.Generic;
using UnityEngine;
using FishTools;
using FishToolsEditor;

/// <summary>
/// 动态UI管理器
/// </summary>

namespace EasyUI
{
    public class UIManager_develop : BaseSingletonMono<UIManager_develop>
    {
        //资源路径
        internal Dictionary<string, string> pathDict = new Dictionary<string, string>();

        //预制件内存
        internal Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

        //已打开界面缓存引用
        internal Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();

        //根节点定义 用于定位UI预制体的父对象位置
        private Transform _uiRoot;
        public Transform UIRoot
        {
            get
            {
                if (_uiRoot == null)
                {
                    //设置ui根节点
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                return _uiRoot;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            LoadPath<FollowPanel>();
            LoadPath<DefaultPanel>();
        }
        //加载资源路径
        void LoadPath<T>() where T : BasePanel
        {
            var datas = Resources.LoadAll<BasePanelData<T>>("");
            foreach (var data in datas)
            {
                data?.AddToPathDict();
            }
        }

        //获取预制件存到内存中
        public GameObject TryGetPrefab(string name)
        {
            //检查路径缓存
            if (!pathDict.TryGetValue(name, out var path))
            {
                DebugEditor.LogError("界面名称错误，或未配置路径:" + name);
                return null;
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
                        DebugEditor.LogError("无法加载预制件,请检查路径是否正确:" + path);
                        return null;
                    }

                    //加入到缓存路径，表示已经加载到内存的预制件
                    prefabDict.Add(name, panelPrefab);
                }
                catch (System.Exception ex)
                {
                    DebugEditor.LogError("加载预制件失败: " + path + "\n" + ex.Message);
                    return null;
                }
            }

            return panelPrefab;
        }

        //打开界面
        public BasePanel OpenPanel(string name)
        {
            //检查已打开页面缓存
            if (panelDict.TryGetValue(name, out var panel))
            {
                DebugEditor.LogWarning("界面已打开:" + name);
                //返回该界面
                return panel;
            }

            //读取预制件内存
            var panelPrefab = TryGetPrefab(name);

            //从内存中加载界面
            if (panelPrefab != null)
            {
                GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);

                panel = panelObject.GetComponent<BasePanel>();
                if (panel == null)
                {
                    DebugEditor.LogError("预制件上没有找到 BasePanel 组件:" + name);
                    Destroy(panelObject);
                    return null;
                }

                //加入到缓存路径，表示已经显示的界面
                panelDict.Add(name, panel);

                DebugEditor.Log(panel.GetInstanceID());
                DebugEditor.Log(panel.gameObject.GetInstanceID());

                return panel;
            }
            else
            {
                return null;
            }
        }

        //关闭界面
        public bool ClosePanel(string name)
        {
            // 检查是否打开并获取面板
            if (panelDict.TryGetValue(name, out var panel))
            {
                // 移除缓存，表示界面未打开
                panelDict.Remove(name);
                // 关闭界面
                panel.Close();
                return true;
            }

            //界面未打开,警告
            DebugEditor.LogWarning("界面未打开:" + name);
            return false;
        }

        //如果关闭则打开，如果打开则关闭
        public BasePanel RepatPanel(string name)
        {
            //如果界面已打开
            if (panelDict.TryGetValue(name, out var panel))
            {
                // 移除缓存，表示界面未打开
                panelDict.Remove(name);
                // 关闭界面
                panel.Close();
                return null;
            }
            //如果界面未打开, 则打开新界面
            return OpenPanel(name);
        }

        //重新打开一个新的UI界面(加载预制体初始的设置)
        public BasePanel OpenNewPanel(string name)
        {
            //如果界面已打开
            if (panelDict.TryGetValue(name, out var panel))
            {
                // 移除缓存，表示界面未打开
                panelDict.Remove(name);
                // 关闭界面
                panel.Close();
                //重新打开新界面
                return OpenPanel(name);
            }
            //如果界面未打开, 则打开新界面
            return OpenPanel(name);
        }

    }
}
