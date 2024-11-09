using UnityEditor;

using UnityEngine;
/// <summary>
/// 只读,编辑器中无法编辑
/// </summary>

namespace FishTools
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 开始禁用 GUI
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label);
            // 结束禁用 GUI
            EditorGUI.EndDisabledGroup();
        }
    }
#endif

}
