using UnityEditor;
using TimerTool;

/// <summary>
/// 计时器管理器编辑器自定义
/// </summary>

namespace FishTools
{
    [CustomEditor(typeof(FTM))]
    public class FTimerManagerEditor : Editor
    {
        private FTM timerManager;
        private bool isDirty = true;

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
            if (isDirty)
            {
                Repaint();
                isDirty = false;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("计时器");

            for (int i = 0; i < timerManager.TimerList.Count; i++)
            {
                FTimer timer = timerManager.TimerList[i];
                EditorGUILayout.FloatField($"timer[{i}]", timer.DisplayTime);
                EditorGUILayout.IntField("循环次数", timer.ResultLoop);
                EditorGUILayout.CurveField("事件曲线",timer.Curve);

                if (!timer.IsCompleted)
                {
                    isDirty = true;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}