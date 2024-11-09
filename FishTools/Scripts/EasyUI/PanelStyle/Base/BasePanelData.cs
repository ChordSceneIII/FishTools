using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Data文件 写 控制UIPanel相关的方法
/// </summary>

namespace FishTools.EasyUI
{
    public abstract class BasePanelData<T> : ScriptableObject, IBasePanelData where T : BasePanel
    {
        [SerializeField, Label("根画布")]
        internal string rootCanvas;
        [SerializeField, Label("面板")]
        internal T basepanel;

        [SerializeField, Label("名称"), ReadOnly]
        internal string panelname;

        [SerializeField, Label("路径"), ReadOnly]
        internal string path;

        // 加载路径到缓存
        public void AddToPathDict()
        {
            UIManager_develop.Instance.pathDict[panelname] = path;
        }


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (basepanel != null)
            {
                // 获取 BasePanel 实例的路径
                string panelPath = AssetDatabase.GetAssetPath(basepanel);

                // 检查路径中是否包含 "Resources/"
                if (!panelPath.Contains("Resources/"))
                {
                    DebugF.LogWarning("预制体必须要在Resources文件夹目录下");
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

                if (rootCanvas != basepanel.rootCanvas)
                {
                    basepanel.rootCanvas = rootCanvas;
                }

                EditorUtility.SetDirty(basepanel);
            }
        }
#endif
    }
}
