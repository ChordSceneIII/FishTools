using UnityEditor;
using UnityEngine;

/// <summary>
/// 仅限于GameObject类型使用,用于检测是否包含指定脚本
/// </summary>

namespace FishToolsEditor
{
    public class RequireScriptAttribute : PropertyAttribute
    {
        public System.Type RequiredType { get; private set; }

        public RequireScriptAttribute(System.Type requiredType)
        {
            RequiredType = requiredType;
        }
    }


    [CustomPropertyDrawer(typeof(RequireScriptAttribute))]
    public class RequireComponentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequireScriptAttribute requireComponentAttribute = (RequireScriptAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);


            // 检查字段类型是否为 ObjectReference，并且引用是否为 GameObject 类型
            if (property.propertyType != SerializedPropertyType.ObjectReference || !(property.objectReferenceValue is GameObject))
            {
                MonoScript script = MonoScript.FromMonoBehaviour(property.serializedObject.targetObject as MonoBehaviour);
                string path = AssetDatabase.GetAssetPath(script);

                // 如果字段不是 GameObject，显示错误信息
                EditorGUI.HelpBox(position, $"{fieldInfo.Name} 必须是 GameObject 类型才能使用 RequireScriptAttribute！", MessageType.Error);
                // 输出详细的错误信息到 Console
                Debug.LogError($"{fieldInfo.Name}必须是 GameObject 类型才能使用 RequireScriptAttribute！路径: <color=aqua>{path}</color>");
                return;
            }

            // 绘制默认的GameObject字段
            EditorGUI.PropertyField(position, property, label);

            // 检查GameObject是否附带指定组件
            if (property.objectReferenceValue != null)
            {
                GameObject gameObject = property.objectReferenceValue as GameObject;
                if (gameObject != null && gameObject.GetComponent(requireComponentAttribute.RequiredType) == null)
                {

                    string path = AssetDatabase.GetAssetPath(gameObject);
                    Debug.LogError($"{fieldInfo.Name} 必须包含 {requireComponentAttribute.RequiredType.Name} 组件！路径: <color=aqua>{path}</color>");


                    // 如果未找到指定的脚本，显示警告
                    GUIStyle helpBoxStyle = EditorStyles.helpBox;
                    string message = $"GameObject 必须包含 {requireComponentAttribute.RequiredType.Name} 组件！";

                    // 计算警告消息的大小
                    Vector2 textSize = helpBoxStyle.CalcSize(new GUIContent(message));

                    // 根据文本宽度，从右往左调整 position.x
                    position.x = position.xMax - textSize.x;

                    // 绘制 HelpBox
                    EditorGUI.HelpBox(position, message, MessageType.Error);

                }
            }

            EditorGUI.EndProperty();
        }
    }

}
