using UnityEngine;
using UnityEditor;

namespace FishToolsEditor
{
    public class SUIConfig_EditorGUI
    {
        [InitializeOnLoadMethod]
        static void InitialzeOnLoad()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }

        static void OnProjectWindowItemGUI(string guid, Rect rect)
        {
            // 确保当前选择的对象不为空
            if (Selection.activeObject == null)
                return;

            // 确保当前选择的对象是有效资源路径
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(assetPath))
                return;

            // 获取当前选择对象的GUID
            string objGUID = AssetDatabase.AssetPathToGUID(assetPath);

            if (!string.IsNullOrEmpty(objGUID) && objGUID == guid && Selection.activeObject.name == "SceneUI")
            {
                rect.x = rect.width - 50;
                rect.width = 100;

                if (GUI.Button(rect, "配置"))
                {
                    EditorWindow.GetWindow<SUIConfig_Windows>().Show();
                }
            }
        }
    }
}

