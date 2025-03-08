#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// InputManager 的自定义编辑器 :显示键位映射表的粗略信息
/// </summary>
namespace FishTools.InputTool
{
    [CustomEditor(typeof(InputManager))]
    public class InputManagerEditor : Editor
    {
        private InputManager inputManager;
        private Vector2 scrollPosition;

        // 折叠状态
        private bool isFoldoutOpen = true;

        private void OnEnable()
        {
            inputManager = (InputManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // 折叠标签
            isFoldoutOpen = EditorGUILayout.Foldout(isFoldoutOpen, "键位映射表", true);

            if (isFoldoutOpen)
            {
                // 设置滚动视图的起始点
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

                // 显示所有的 Mapper
                foreach (KeyValuePair<string, KeyMapper> entry in GetMapperDic())
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Name: " + entry.Key, GUILayout.Width(200));
                    EditorGUILayout.LabelField("Type: " + entry.Value.GetType().Name, GUILayout.Width(200));
                    EditorGUILayout.EndHorizontal();
                }

                // 结束滚动视图
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.Space();
        }

        private Dictionary<string, KeyMapper> GetMapperDic()
        {
            // 使用 InputManager.Instance 访问实例字段
            if (InputManager.Instance != null)
            {
                return InputManager.mapperDic;
            }
            return new Dictionary<string, KeyMapper>();
        }
    }
}
#endif