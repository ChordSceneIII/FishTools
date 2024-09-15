using UnityEditor;
using UnityEngine;
/// <summary>
/// 只读，使用public加该特性生效,带SerializedField的字段无效
/// </summary>

namespace FishToolsEditor
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

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
}

//////
////// 不需要序列化就可以实现的只读方案,利用反射对Mono编辑器修改
//////

// using UnityEngine;
// using UnityEditor;
// using System.Reflection;
// using System;

// namespace FishToolsEditor
// {
//     // 自定义的 ReadOnly 特性
//     [AttributeUsage(AttributeTargets.Field)]
//     public class ReadOnlyAttribute : PropertyAttribute
//     {
//     }

//     [CustomEditor(typeof(MonoBehaviour), true)]
//     public class ReadOnlyEditor : Editor
//     {
//         private object lastFieldValue; // 用于缓存字段的上一个值

//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();//重写mono的Editor会导致可序列化类即使没有可序列化字段也会显示器类名于Inspector中
//             serializedObject.Update();

//             // 遍历所有字段
//             var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

//             foreach (FieldInfo field in fields)
//             {
//                 // 检查字段是否具有 ReadOnlyAttribute 特性
//                 ReadOnlyAttribute readOnlyAttr = field.GetCustomAttribute<ReadOnlyAttribute>();

//                 if (readOnlyAttr != null)
//                 {
//                     // 获取字段的当前值
//                     object currentValue = field.GetValue(target);

//                     // 如果字段值发生变化，触发刷新
//                     if (!Equals(lastFieldValue, currentValue))
//                     {
//                         lastFieldValue = currentValue;  // 更新缓存的字段值
//                         Repaint();  // 强制刷新 Inspector
//                     }

//                     // 显示只读字段
//                     EditorGUI.BeginDisabledGroup(true);
//                     EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(field.Name), currentValue != null ? currentValue.ToString() : "null");
//                     EditorGUI.EndDisabledGroup();
//                 }
//             }

//             // 应用修改并强制刷新 Inspector
//             serializedObject.ApplyModifiedProperties();
//         }
//     }
// }