using UnityEngine;
using UnityEditor;

namespace FishTools.SceneUI
{
    public class SUIConfig_Windows : EditorWindow
    {
        private const string configFolderPath = "Assets/Resources";
        private const string configFileName = "SceneUIConfig.asset";
        private SceneUIConfig config;

        [MenuItem("FishTools/Open Config Window")]
        public static void ShowWindow()
        {
            GetWindow<SUIConfig_Windows>("SceneUI配置");
        }

        private void OnEnable()
        {
            // 尝试加载现有的 SceneUIConfig 实例
            LoadConfig();
        }

        private void LoadConfig()
        {
            string fullPath = $"{configFolderPath}/{configFileName}";
            config = AssetDatabase.LoadAssetAtPath<SceneUIConfig>(fullPath);
        }

        private void OnGUI()
        {
            if (config == null)
            {
                // 如果没有现有的 SceneUIConfig 实例，显示创建按钮
                EditorGUILayout.LabelField("No SceneUIConfig found. Create a new one:");
                if (GUILayout.Button("Create SceneUIConfig"))
                {
                    CreateConfig();
                }
            }
            else
            {
                // 显示已存在的 SceneUIConfig 实例
                EditorGUILayout.LabelField("SceneUIConfig already exists:");
                EditorGUILayout.ObjectField(config, typeof(SceneUIConfig), false);
            }
        }

        private void CreateConfig()
        {
            string fullPath = $"{configFolderPath}/{configFileName}";
            config = ScriptableObject.CreateInstance<SceneUIConfig>();
            AssetDatabase.CreateAsset(config, fullPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;
        }
    }
}
