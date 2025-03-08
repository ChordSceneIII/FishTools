#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// 计时器管理器编辑器自定义
/// </summary>

namespace FishTools.TimeTool
{
    [CustomEditor(typeof(FTM))]
    public class FTMEditor : Editor
    {
        private FTM timerManager;

        private void OnEnable()
        {
            timerManager = (FTM)target;
            EditorApplication.update += OnEditorUpdate;
        }
        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
        private void OnEditorUpdate()
        {
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter, // 设置文本居中
                fontSize = 13, // 设置字体大小
                fontStyle = FontStyle.Bold // 设置字体样式
            };

            EditorGUILayout.LabelField("计时器列表", centeredStyle, GUILayout.ExpandWidth(true));

            for (int i = 0; i < timerManager.TimerList.Count; i++)
            {
                FTimer timer = timerManager.TimerList[i];
                EditorGUILayout.LabelField($"timer[{i}]");
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.FloatField($"计时", timer.DisplayTime);
                EditorGUILayout.IntField("循环次数", timer.ResultLoop);
                EditorGUILayout.CurveField("事件曲线", timer.Curve);
                EditorGUILayout.EndVertical();
            }
        }
    }
}
#endif