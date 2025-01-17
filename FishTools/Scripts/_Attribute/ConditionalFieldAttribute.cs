using System;
using UnityEditor;
using UnityEngine;

namespace FishTools
{
    /// <summary>
    /// 条件对比（字段，比较值）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string ConditionPropertyName { get; private set; }
        public object CompareValue { get; private set; }

        public ConditionalFieldAttribute(string conditionPropertyName, object compareValue)
        {
            ConditionPropertyName = conditionPropertyName;
            CompareValue = compareValue;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditionalAttribute = (ConditionalFieldAttribute)attribute;

            // 获取目标字段的值
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionalAttribute.ConditionPropertyName);

            // 如果字段不存在，显示 HelpBox 并返回
            if (conditionProperty == null)
            {
                Rect helpBoxRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 2);
                EditorGUI.HelpBox(helpBoxRect, $"Field '{conditionalAttribute.ConditionPropertyName}' not found.", MessageType.Error);
                return;
            }

            // 获取字段的实际值
            object fieldValue = GetPropertyValue(conditionProperty);

            // 比较字段值和指定参数
            bool conditionMet = CompareValues(fieldValue, conditionalAttribute.CompareValue);

            // 如果条件满足，则绘制字段
            if (conditionMet)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditionalAttribute = (ConditionalFieldAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionalAttribute.ConditionPropertyName);

            // 如果字段不存在，返回 HelpBox 的高度
            if (conditionProperty == null)
            {
                return EditorGUIUtility.singleLineHeight * 2; // HelpBox 高度
            }

            object fieldValue = GetPropertyValue(conditionProperty);
            bool conditionMet = CompareValues(fieldValue, conditionalAttribute.CompareValue);

            if (!conditionMet)
            {
                // 如果条件不满足，返回 0 高度（不绘制）
                return 0;
            }

            return EditorGUI.GetPropertyHeight(property, label);
        }

        private object GetPropertyValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Enum:
                    return GetEnumValue(property);
                default:
                    return null;
            }
        }

        //获取枚举值，（防止Flag标签信息丢失）
        private int GetEnumValue(SerializedProperty property)
        {
            // 获取字段的类型
            var fieldInfo = property.serializedObject.targetObject.GetType().GetField(property.propertyPath);
            if (fieldInfo != null && fieldInfo.FieldType.IsEnum)
            {
                // 直接返回枚举值的 int 表示
                return property.intValue;
            }
            return -1; // 返回一个无效值，表示不是枚举类型
        }

        private bool CompareValues(object fieldValue, object compareValue)
        {
            if (fieldValue == null || compareValue == null)
            {
                return fieldValue == compareValue;
            }

            // 如果 compareValue 是枚举类型
            if (compareValue is Enum compareEnum)
            {
                // 获取枚举类型
                Type enumType = compareEnum.GetType();

                // 检查是否是 Flags 枚举
                bool isFlagsEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

                // 将 compareEnum 转换为 int
                int compareInt = Convert.ToInt32(compareEnum);

                // 如果 fieldValue 是整数，进行比较
                if (fieldValue is int fieldInt)
                {
                    if (isFlagsEnum)
                    {
                        // 对于 Flags 枚举，使用位运算
                        return (fieldInt & compareInt) == compareInt;
                    }
                    else
                    {
                        // 对于普通枚举，直接比较值
                        return fieldInt == compareInt;
                    }
                }
            }

            // 默认比较方式
            return fieldValue.Equals(compareValue);
        }
    }
#endif
}
