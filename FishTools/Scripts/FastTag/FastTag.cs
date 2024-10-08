using System.Collections.Generic;
using System.Linq;
using FishToolsEditor;
using UnityEngine;
/// <summary>
/// 快速标签，Unity FindObjectwithTag的上位替代 (但是只支持运行环境不支持编辑器环境的使用)
/// 与Tag不同，该方法的Tag是唯一的，每个实例只能有唯一的Tag，利用率Dic的key不重复的特性
/// 而且可以自主在生命周期中声明注册和注销时机
/// 具有唯一性，快速性，和灵活性
/// </summary>

namespace FishTools
{
    public static class FastTag<T> where T : MonoBehaviour, IFastTag
    {
        //这个字典并非全局唯一，而是全类型T唯一
        private static Dictionary<string, T> allMonoObjects = new Dictionary<string, T>();

        public static void Register(T obj)
        {
            if (!allMonoObjects.ContainsKey(obj.FID))
            {
                allMonoObjects[obj.FID] = obj;
            }
            else
            {
                DebugEditor.LogWarning($"Object with ID {obj.FID} is already registered.");
            }
        }

        public static void UnRegister(T obj)
        {
            if (allMonoObjects.ContainsKey(obj.FID))
            {
                allMonoObjects.Remove(obj.FID);
            }
        }

        public static T FindByID(string id)
        {
            allMonoObjects.TryGetValue(id, out var obj);
            return obj;
        }

        public static T[] FindALL()
        {
            return allMonoObjects.Values.ToArray();
        }

        public static IReadOnlyDictionary<string, T> GetAllFastTagObjects()
        {
            return allMonoObjects;
        }
    }
}