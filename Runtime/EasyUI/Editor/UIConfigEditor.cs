#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FishTools.EasyUI
{
    [CustomEditor(typeof(UIConfig))]
    public class UIConfigEditor : Editor
    {
        UIConfig config;
        string key;
        string panelname;
        private void OnEnable()
        {
            config = (UIConfig)target;
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("索引", GUILayout.Width(40));
            key = EditorGUILayout.TextField(key, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("名", GUILayout.Width(40));
            panelname = EditorGUILayout.TextField(panelname, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("打开面板"))
            {
                config.ForcePanel(key, panelname, true);
            }

            if (GUILayout.Button("关闭面板"))
            {
                config.ClosePanel(key);
            }

            EditorGUILayout.EndVertical();
            if (GUILayout.Button("清理面板和缓存"))
            {
                config.ClearAllPanels();
            }
        }
    }
}
#endif