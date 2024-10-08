using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace FishToolsEditor
{
     public static class DebugEditor
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
    }
}

