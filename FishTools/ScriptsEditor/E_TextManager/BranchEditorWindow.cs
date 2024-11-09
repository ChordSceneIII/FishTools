using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishTools.TextManager
{
    public class BranchEditorWindow : EditorWindow
    {
        private TextPlayer player;  // 播放器
        private SerializedObject serializedPlayer;
        private SerializedProperty branch;
        private SerializedProperty nodeList;
        private Vector2 scrollPos; // 滚动视图的位置
        private string _branchName; // 分支名称
        public static void OpenWindow(TextPlayer player)
        {
            if (player == null)
            {
                return;
            }
            BranchEditorWindow window = GetWindow<BranchEditorWindow>("分支内容编辑器");
            window.Show();
            window.player = player;
        }

        private void OnGUI()
        {
            if (player == null)
            {
                EditorGUILayout.LabelField("请选择一个 TextPlayer 对象");
                return;
            }

            serializedPlayer = new SerializedObject(player);
            branch = serializedPlayer.FindProperty("currentBranch");
            nodeList = branch.FindPropertyRelative("nodeList");

            #region 界面设计
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("当前路径:", GUILayout.Width(100));
            EditorGUILayout.LabelField(BranchSave.Path);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("分支名称:", GUILayout.Width(100));
            _branchName = EditorGUILayout.TextField(_branchName);
            if (GUILayout.Button("从JSON中加载"))
            {
                player.LoadBranch(_branchName);
            }
            if (GUILayout.Button("保存到JSON"))
            {
                player.SaveCurBranch(_branchName);
            }
            EditorGUILayout.EndHorizontal();

            // 开始滚动视图
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // 存储要删除的索引
            List<int> indicesToRemove = new List<int>();

            // 遍历并显示 nodeList
            for (int j = 0; j < nodeList.arraySize; j++)
            {
                SerializedProperty node = nodeList.GetArrayElementAtIndex(j);
                SerializedProperty text = node.FindPropertyRelative("text");
                SerializedProperty actionEvent = node.FindPropertyRelative("actionEvent");
                SerializedProperty name = node.FindPropertyRelative("name");

                // 使用水平布局
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"[{j}]", GUILayout.Width(30));
                EditorGUILayout.LabelField("角色:", GUILayout.Width(50));
                name.stringValue = EditorGUILayout.TextField(name.stringValue, GUILayout.Width(120)); // 设置角色框的宽度

                // 删除按钮放在右侧
                GUILayout.FlexibleSpace(); // 填充空间
                if (GUILayout.Button("删除", GUILayout.Width(50)))
                {
                    indicesToRemove.Add(j); // 添加到待删除列表
                }
                EditorGUILayout.EndHorizontal();

                text.stringValue = EditorGUILayout.TextArea(text.stringValue, GUILayout.Height(EditorStyles.textArea.CalcHeight(new GUIContent(text.stringValue), EditorGUIUtility.currentViewWidth)));
                EditorGUILayout.PropertyField(actionEvent, true); // 展开并显示 actionEvent 的属性

                // 绘制分隔线
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            // 删除节点
            for (int i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                nodeList.DeleteArrayElementAtIndex(indicesToRemove[i]);
            }

            // 添加对话按钮
            if (GUILayout.Button("添加对话"))
            {
                player.currentBranch.nodeList.Add(new TextNode("character", "text content"));
            }

            // 结束滚动视图
            EditorGUILayout.EndScrollView();

            #endregion

            serializedPlayer.ApplyModifiedProperties(); // 应用更改
        }



    }
}
