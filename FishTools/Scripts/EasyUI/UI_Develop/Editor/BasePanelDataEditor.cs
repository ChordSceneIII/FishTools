using UnityEditor;

/// <summary>
/// UI数据编辑器: 自动获取预制体路径
/// </summary>

namespace EasyUI
{
    [CustomEditor(typeof(BasePanelData), true)]
    public abstract class BasePanelDataEditor : Editor
    {
        protected BasePanelData data;

        public override void OnInspectorGUI()
        {
            //绘制默认Inspector
            DrawDefaultInspector();

            data = (BasePanelData)target;

            if (data.panelPrefab != null)
            {
                //获取预制体的路径
                string path = AssetDatabase.GetAssetPath(data.panelPrefab.gameObject);

                //检查路径中是否包含"Resources/"
                if (path.Contains("Resources/"))
                {
                    //获取在Resources文件夹中的路径
                    int startIndex = path.IndexOf("Resources/") + 10;
                    int length = path.LastIndexOf('.') - startIndex;
                    path = path.Substring(startIndex, length);

                    //更新路径
                    data.path = path;

                    //更新面板名字
                    data.panelname = data.panelPrefab.name;
                    data.panelPrefab.panelname = data.panelPrefab.name;

                    //标记为已更改
                    EditorUtility.SetDirty(data);
                }
                else
                {
                    EditorGUILayout.HelpBox("The prefab must be in a Resources folder.\n预制体必须要在Resources文件夹目录下", MessageType.Warning);
                }
            }
        }
    }
}