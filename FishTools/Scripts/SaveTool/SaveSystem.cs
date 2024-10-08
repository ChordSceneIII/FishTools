using UnityEngine;
using System.IO;
using FishToolsEditor;

/// <summary>
/// 存档系统底层逻辑，通常需要创建一个SaveManager来管理存档的配置相关
/// </summary>

namespace FishTools
{
    public static class SaveSystem
    {
        //写入JSON文件
        public static void SaveToJson(string path, object data)
        {
            var json = JsonUtility.ToJson(data);

            try
            {
                File.WriteAllText(path, json);
                DebugEditor.Log($"Successfully save to <color=#00AAFF>{path}</color>");
            }

            catch (System.Exception exce)
            {
                DebugEditor.LogError($"Failed to save file to {path} \n {exce}");
            }
        }

        //文件读取与JSON解析
        public static T LoadFromJSON<T>(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                DebugEditor.Log($"Successfully load from <color=#00AAFF>{path}</color>");
                return data;
            }
            catch (System.Exception exce)
            {
                DebugEditor.LogError($"Failed to load from {path} \n {exce}");
                return default(T);//返回默认值,用于泛型方法的返回
            }
        }

        //删除JSON文件
        public static void DeleteJSON(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (System.Exception exce)
            {
                DebugEditor.LogError($"Failed to delete file from  {path} \n {exce}");
            }
        }
    }
}
