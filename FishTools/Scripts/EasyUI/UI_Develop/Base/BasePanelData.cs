using FishToolsEditor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Data文件 写 控制UIPanel相关的方法
/// </summary>

namespace EasyUI
{
    public abstract class BasePanelData<T> : ScriptableObject
        where T : BasePanel
    {
        [SerializeField, Label("面板类型 ")]
        internal T basepanel;

        [SerializeField, Label("名称"), ReadOnly]
        internal string panelname;

        [SerializeField, Label("路径"), ReadOnly]
        internal string path;

        // 加载路径到缓存
        internal void AddToPathDict()
        {
            UIManager_develop.Instance.pathDict.Add(this.panelname, this.path);
        }

        //使用默认的回调方式
        protected void BaseDeClose()
        {
            UIManager_develop.Instance.ClosePanel(panelname);
        }

        protected void BaseDeOpen()
        {
            UIManager_develop.Instance.OpenPanel(panelname);
        }

        protected void BaseDeRepat()
        {
            UIManager_develop.Instance.RepatPanel(panelname);
        }

        //使用UI对象池的方式 (可创建多个实例,通过refname作为key)
        protected int BasePoGetNewPanel()
        {
            var refID = UIManager_pool.Instance?.GetNewPanel(panelname).Item2;
            return refID ?? default;
        }

        protected void BasePoClose(int refID)
        {
            UIManager_pool.Instance?.ClosePanel(refID);
        }

        protected void BasePoOpen(int refID)
        {
            UIManager_pool.Instance?.OpenPanel(refID);
        }

        protected void BasePoRepeat(int refID)
        {
            UIManager_pool.Instance?.RepatPanel(refID);
        }

        //路径自动化配置
        private void OnValidate()
        {
            if (basepanel != null)
            {
                // 获取 BasePanel 实例的路径
                string panelPath = AssetDatabase.GetAssetPath(basepanel);

                // 检查路径中是否包含 "Resources/"
                if (!panelPath.Contains("Resources/"))
                {
                    DebugEditor.LogWarning(
                        "The prefab must be in a Resources folder.\n预制体必须要在Resources文件夹目录下"
                    );
                    return;
                }

                // 获取在 Resources 文件夹中的路径
                int startIndex = panelPath.IndexOf("Resources/") + 10;
                int length = panelPath.LastIndexOf('.') - startIndex;
                string newPath = panelPath.Substring(startIndex, length);

                // 避免频繁更新路径
                if (path != newPath)
                {
                    path = newPath;
                }

                if (panelname != basepanel.name)
                {
                    panelname = basepanel.name;
                }

                // 仅当确实发生更改时才标记为已修改
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(this);
                }
            }
        }
    }
}
