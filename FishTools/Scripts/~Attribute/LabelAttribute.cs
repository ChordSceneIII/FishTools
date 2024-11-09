using UnityEditor;

using UnityEngine;

/// <summary>
/// 属性名更改
/// </summary>
namespace FishTools
{
    public class LabelAttribute : PropertyAttribute
    {
        public string Label { get; private set; }
        public LabelAttribute(string label)
        {
            Label = label;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = (LabelAttribute)attribute;

            // 使用原始标签的文本和内容，防止干扰排列
            GUIContent newLabel = new GUIContent(labelAttribute.Label);

            // 绘制属性字段
            EditorGUI.PropertyField(position, property, newLabel, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 返回默认高度，确保不会影响布局
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
}