using UnityEditor;
using System.Collections.Generic;

namespace FishTools.ActionSystem
{

    [CustomEditor(typeof(ActionManager))]
    public class ActionManagerEditor : Editor
    {
        private SerializedProperty commandListProp;

        private void OnEnable()
        {
            // 获取序列化属性
            commandListProp = serializedObject.FindProperty("commandList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 绘制默认的检查器
            DrawDefaultInspector();

            CheckForDuplicateCommandNames();


            serializedObject.ApplyModifiedProperties();
        }

        private void CheckForDuplicateCommandNames()
        {
            var commandNames = new HashSet<string>();

            for (int i = 0; i < commandListProp.arraySize; i++)
            {
                var item = commandListProp.GetArrayElementAtIndex(i);
                var command = item.objectReferenceValue as BaseCommand;

                if (command != null)
                {
                    if (!commandNames.Add(command.command_name))
                    {
                        // 如果重复，显示警告
                        EditorGUILayout.HelpBox($"重复的命令名: {command.command_name}", MessageType.Error);
                    }
                }
            }
        }
    }
}