using UnityEngine;
using System.IO;

/// <summary>
/// 存档系统底层逻辑，通常需要创建一个SaveManager来管理存档的配置相关
/// </summary>

namespace FishTools.SaveTool
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
                DebugF.Log($"成功保存JSON数据: <color=#00AAFF>{path}</color>");
            }

            catch (System.Exception exce)
            {
                DebugF.LogError($"保存JSON数据失败: <color=#00AAFF>{path}</color> \n {exce}");
            }
        }

        //文件读取与JSON解析
        public static T LoadFromJSON<T>(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                DebugF.Log($"成功加载JSON数据: <color=#00AAFF>{path}</color>");
                return data;
            }
            catch (System.Exception exce)
            {
                DebugF.LogError($"加载JSON数据失败: <color=#00AAFF>{path}</color>\n {exce}");
                return default(T);//返回默认值,用于泛型方法的返回
            }
        }

        //删除JSON文件
        public static void DeleteJSON(string path)
        {
            try
            {
                File.Delete(path);
                DebugF.LogError($"成功删除数据: <color=#00AAFF>{path}</color> \n");

            }
            catch (System.Exception exce)
            {
                DebugF.LogError($"删除数据失败: <color=#00AAFF>{path}</color> \n {exce}");
            }
        }
    }
}
