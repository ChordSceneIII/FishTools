using System.Diagnostics;
using UnityEngine;

namespace FishTools
{
    public static class DebugF
    {
        // 控制日志输出的开关
        public static bool EnableLogging { get; set; } = true;

        [Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            if (EnableLogging)
            {
                UnityEngine.Debug.Log(message);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            if (EnableLogging)
            {
                UnityEngine.Debug.LogWarning(message);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            if (EnableLogging)
            {
                UnityEngine.Debug.LogError(message);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogColor(string colorStr, object message)
        {
            if (EnableLogging)
            {
                // 转换枚举为颜色字符串
                string colorMessage = $"<color={colorStr}>{message}</color>";
                UnityEngine.Debug.Log(colorMessage);
            }
        }
    }
}

