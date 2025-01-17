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
            EditorGUI.PropertyField(position, property, label, true);
            // 结束禁用 GUI
            EditorGUI.EndDisabledGroup();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 返回默认高度，确保不会影响布局
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif

}
