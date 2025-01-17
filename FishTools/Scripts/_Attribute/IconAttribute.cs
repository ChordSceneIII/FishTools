using UnityEditor;
using UnityEngine;

namespace FishTools
{
    public class IconFieldAttribute : PropertyAttribute
    {
        public string iconName;

        public IconFieldAttribute(string iconName)
        {
            this.iconName = iconName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(IconFieldAttribute))]
    public class IconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 获取 IconAttribute
            IconFieldAttribute iconAttribute = (IconFieldAttribute)attribute;

            // 加载图标
            GUIContent iconContent = EditorGUIUtility.IconContent(iconAttribute.iconName);

            // 显示图标
            if (iconContent != null && iconContent.image != null)
            {
                Rect iconRect = new Rect(position.x, position.y, 16, 16);
                GUI.Label(iconRect, iconContent);
            }

            // 显示默认属性
            Rect propertyRect = new Rect(position.x + 20, position.y, position.width - 20, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);
        }
    }
#endif
}
