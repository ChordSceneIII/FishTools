using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// FastTag 用于在 Unity 中快速查找和管理 MonoBehaviour 实例的工具。
/// 使用需要实现 IFastTag 接口并声明 FID，即可自动将对象注册到全局字典。
/// 支持通过 FID 查找、自动注册、动态缓存，无需手动管理，在编辑器和运行时都能正常工作。
/// </summary>

namespace FishTools
{
    public interface IFastTag<T> where T : MonoBehaviour, IFastTag<T>
    {
        // 用于标识对象的唯一标识符
        string FID { get; }

        // 接口默认实现，用于自动注册
        void Register()
        {
            FastTag<T>.Register(this as T);
        }
    }

    public static class FastTag<T> where T : MonoBehaviour, IFastTag<T>
    {
        private static Dictionary<string, T> scripts = new Dictionary<string, T>();

        public static void Register(T mono)
        {
            if (mono == null)
            {
                DebugF.LogWarning("尝试注册空对象");
                return;
            }

            //检查是否重复注册
            if (!scripts.TryGetValue(mono.FID, out var m_obj))
            {
                scripts[mono.FID] = mono;
            }
            else
            {
                DebugF.LogWarning("重复注册ID为" + mono.FID + "的脚本");
            }
        }

        public static T FindByID(string id)
        {
            if (TryGetTag(id, out var mono))
            {
                // DebugF.Log($"找到{mono}");
                return mono;
            }
            else
            {
                DebugF.LogWarning("找不到ID为" + id + "的脚本");
                return null;
            }

        }

        public static bool ContainsID(string id)
        {
            return TryGetTag(id, out var mono);
        }

        public static T[] FindALL()
        {
            var cur_scripts = GameObject.FindObjectsOfType<T>();
            foreach (var cur_script in cur_scripts)
            {
                cur_script.Register();
            }
            return scripts.Values.ToArray();
        }


        // 核心逻辑 。动态寻访与注册，脱离生命周期管理
        public static bool TryGetTag(string id, out T mono)
        {
            scripts.TryGetValue(id, out var m_obj);

            if (m_obj == null)
            {
                var cur_scripts = GameObject.FindObjectsOfType<T>(true);
                foreach (var cur_script in cur_scripts)
                {
                    if (cur_script.FID == id)
                    {
                        cur_script.Register();
                        mono = cur_script;
                        return true;
                    }
                }
            }

            mono = m_obj;


            if (mono != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

}