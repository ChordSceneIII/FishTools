#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FishTools
{

    [CustomPropertyDrawer(typeof(ListOrdered<>), true)]
    public class OrderedSetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 获取 _list 的 SerializedProperty
            SerializedProperty listProperty = property.FindPropertyRelative("_list");

            EditorGUI.BeginDisabledGroup(true);

            // 绘制 _list
            EditorGUI.PropertyField(position, listProperty, label, true);

            // 结束禁用 UI 交互
            EditorGUI.EndDisabledGroup();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 返回 _list 的高度
            SerializedProperty listProperty = property.FindPropertyRelative("_list");
            return EditorGUI.GetPropertyHeight(listProperty, label, true);
        }
    }
}
#endif