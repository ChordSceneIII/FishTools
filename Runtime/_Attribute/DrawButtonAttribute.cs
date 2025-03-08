using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishTools
{
    /// <summary>
    /// 利用反射绘制 Inspector 按钮 (对 MonoBehaviour 修改)
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Method)]
    public class DrawButtonAttribute : Attribute
    {
        public string ButtonName { get; private set; }
        public bool isPlayMode;

        public DrawButtonAttribute(string buttonName = null, bool isPlayMode = false)
        {
            ButtonName = buttonName;
            this.isPlayMode = isPlayMode;
        }
        public DrawButtonAttribute(bool isPlayMode = false)
        {
            ButtonName = null;
            this.isPlayMode = isPlayMode;
        }
        public DrawButtonAttribute()
        {
            ButtonName = null;
            this.isPlayMode = false;
        }
    }

    #if UNITY_EDITOR
    /// <summary>
    /// 辅助类：使用字典映射实现各类型参数的绘制
    /// </summary>
    public static class EditorGUILayoutHelper
    {
        // 定义绘制委托：接收标签和当前值，返回更新后的值
        public delegate object FieldDrawer(string label, object value);

        // 建立类型到绘制方法的映射字典
        private static readonly Dictionary<Type, FieldDrawer> drawerMapping = new Dictionary<Type, FieldDrawer>
        {
            { typeof(int), (label, value) => EditorGUILayout.IntField(label, (int)value) },
            { typeof(float), (label, value) => EditorGUILayout.FloatField(label, (float)value) },
            { typeof(string), (label, value) => EditorGUILayout.TextField(label, (string)value) },
            { typeof(bool), (label, value) => EditorGUILayout.Toggle(label, (bool)value) },
            { typeof(Vector3), (label, value) => EditorGUILayout.Vector3Field(label, (Vector3)value) },
            // 如需支持其他类型，可在此添加映射
        };

        /// <summary>
        /// 根据类型绘制输入框，并更新对应的值
        /// </summary>
        public static bool TryDrawField(Type type, string label, ref object value)
        {
            if (drawerMapping.TryGetValue(type, out FieldDrawer drawer))
            {
                value = drawer(label, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断当前类型是否支持绘制
        /// </summary>
        public static bool IsTypeSupported(Type type)
        {
            return drawerMapping.ContainsKey(type);
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MethodButtonEditor : Editor
    {
        private MethodInfo[] _cachedMethods; // 缓存方法信息
        // 存储每个方法对应的参数输入值
        private Dictionary<MethodInfo, object[]> methodParameterValues = new Dictionary<MethodInfo, object[]>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_cachedMethods == null)
            {
                // 缓存所有带有 DrawButtonAttribute 的方法
                _cachedMethods = target.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method => method.GetCustomAttribute<DrawButtonAttribute>() != null)
                    .ToArray();
            }

            foreach (var method in _cachedMethods)
            {
                var buttonAttribute = method.GetCustomAttribute<DrawButtonAttribute>();
                if (buttonAttribute != null)
                {
                    string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonName) ? method.Name : buttonAttribute.ButtonName;

                    // 仅在播放模式下显示（如果设置了 isPlayMode）
                    if (buttonAttribute.isPlayMode && !Application.isPlaying)
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = method.GetParameters();
                    // 判断所有参数类型是否都支持
                    bool supported = parameters.All(p => EditorGUILayoutHelper.IsTypeSupported(p.ParameterType));
                    if (!supported)
                    {
                        EditorGUILayout.HelpBox($"{method.Name} 存在不支持的参数类型", MessageType.Error);
                        continue;
                    }

                    // 如果方法参数值未缓存，则初始化默认值
                    if (!methodParameterValues.ContainsKey(method))
                    {
                        object[] defaultValues = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Type type = parameters[i].ParameterType;
                            if (type == typeof(int))
                                defaultValues[i] = 0;
                            else if (type == typeof(float))
                                defaultValues[i] = 0f;
                            else if (type == typeof(string))
                                defaultValues[i] = "";
                            else if (type == typeof(bool))
                                defaultValues[i] = false;
                            else if (type == typeof(Vector3))
                                defaultValues[i] = Vector3.zero;
                        }
                        methodParameterValues[method] = defaultValues;
                    }

                    object[] parameterValues = methodParameterValues[method];

                    // 使用水平布局：先绘制所有参数，再绘制按钮
                    EditorGUILayout.BeginVertical("box");
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        object currentValue = parameterValues[i];
                        // 调用辅助方法绘制控件
                        if (!EditorGUILayoutHelper.TryDrawField(parameters[i].ParameterType, parameters[i].Name, ref currentValue))
                        {
                            EditorGUILayout.LabelField($"不支持类型 {parameters[i].ParameterType}");
                        }
                        parameterValues[i] = currentValue;
                    }
                    if (GUILayout.Button(buttonName))
                    {
                        method.Invoke(target, parameterValues);
                        EditorUtility.SetDirty(target);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
#endif
}
