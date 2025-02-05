using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FishTools
{
    /// <summary>
    /// 利用反射绘制Inspector按钮 (对MonoBehaviour修改)
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Method)] // 限制只能用于方法
    public class DrawButtonAttribute : Attribute
    {
        public string ButtonName { get; private set; } // 按钮显示的名称
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
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MethodButtonEditor : Editor
    {
        private MethodInfo[] _cachedMethods; // 缓存方法信息以减少反射调用

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_cachedMethods == null)
            {
                // 获取所有 Public 实例方法，并缓存带有 DrawButtonAttribute 的方法
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

                    if (buttonAttribute.isPlayMode && !Application.isPlaying) return;

                    // 绘制按钮
                    if (GUILayout.Button(buttonName))
                    {
                        // 调用该方法
                        method.Invoke(target, null);
                        EditorUtility.SetDirty(target);
                    }
                }
            }
        }
    }
#endif
}
