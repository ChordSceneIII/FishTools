using UnityEditor;
using UnityEngine;


namespace FishTools.TextManager
{
    [CustomEditor(typeof(TextPlayer))]
    public class TextPlayerEditor : Editor
    {
        // private (string, int) branchInfo;

        // private string savebranchName;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TextPlayer textplayer = (TextPlayer)target;

            if (GUILayout.Button("播放"))
            {
                textplayer.PlayText();
            }
            if (GUILayout.Button("跳过"))
            {
                textplayer.Skip();
            }
            if (GUILayout.Button("下一页"))
            {
                textplayer.Next();
            }
            if (GUILayout.Button("上一页"))
            {
                textplayer.Last();
            }
            if (GUILayout.Button("自动播放"))
            {
                textplayer.AutoPlay();
            }
            if (GUILayout.Button("停止自动播放"))
            {
                textplayer.StopAutoPlay();
            }

            // EditorGUILayout.BeginVertical("box");
            // EditorGUILayout.LabelField("数据管理");

            // EditorGUILayout.BeginHorizontal();
            // branchInfo.Item1 = EditorGUILayout.TextField("Branch", branchInfo.Item1);
            // branchInfo.Item2 = EditorGUILayout.IntField("Index", branchInfo.Item2);
            // EditorGUILayout.EndHorizontal();
            // if (GUILayout.Button("加载Branch"))
            // {
            //     textplayer.LoadBranch(branchInfo.Item1, branchInfo.Item2);
            // }

            // EditorGUILayout.BeginHorizontal();
            // savebranchName = EditorGUILayout.TextField("保存Branch", savebranchName);
            // if (GUILayout.Button("保存Branch"))
            // {
            //     textplayer.SaveCurBranch(savebranchName);
            // }
            // EditorGUILayout.EndHorizontal();

            // EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (GUILayout.Button("打开编辑窗口"))
            {
                BranchEditorWindow.OpenWindow(textplayer);
            }

        }
    }
}