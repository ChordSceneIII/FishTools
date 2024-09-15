using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// InputManager 的自定义编辑器 :显示键位映射表的粗略信息
/// </summary>

[CustomEditor(typeof(InputTool.InputManager))]
public class InputManagerEditor : Editor
{
    private InputTool.InputManager inputManager;
    private Vector2 scrollPosition;

    // 折叠状态
    private bool isFoldoutOpen = true;

    private void OnEnable()
    {
        inputManager = (InputTool.InputManager)target;
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
            foreach (KeyValuePair<string, InputTool.KeyMapper> entry in GetMapperDic())
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

    private Dictionary<string, InputTool.KeyMapper> GetMapperDic()
    {
        // 使用 InputManager.Instance 访问实例字段
        if (InputTool.InputManager.Instance != null)
        {
            return InputTool.InputManager.mapperDic;
        }
        return new Dictionary<string, InputTool.KeyMapper>();
    }
}
