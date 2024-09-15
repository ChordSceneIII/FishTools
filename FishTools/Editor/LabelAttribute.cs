using UnityEditor;
using UnityEngine;
/// <summary>
/// 属性名更改
/// </summary>

namespace FishToolsEditor
{
    public class LabelAttribute : PropertyAttribute
    {
        public string Label { get; private set; }
        public LabelAttribute(string label)
        {
            Label = label;
        }
    }

    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = (LabelAttribute)attribute;
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.Label));
        }
    }
}
