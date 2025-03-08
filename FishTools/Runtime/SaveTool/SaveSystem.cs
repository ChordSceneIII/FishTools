using UnityEngine;
using System.IO;
using UnityEditor;

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
                // 检查文件是否存在
                if (File.Exists(path))
                {
                    File.Delete(path);
                    DebugF.Log($"成功删除数据: <color=#00AAFF>{path}</color> \n");
                }
                else
                {
                    DebugF.LogWarning($"文件不存在，无法删除: <color=#00AAFF>{path}</color> \n");
                }
            }
            catch (System.Exception exce)
            {
                DebugF.LogError($"删除数据失败: <color=#00AAFF>{path}</color> \n {exce}");
            }
        }


        // 返回指定文件夹路径，如果路径不存在则自动创建
        public static string Path(string folderName)
        {
            string basePath;

#if UNITY_EDITOR
            basePath = Application.dataPath;  // 编辑器下的路径
#else
        basePath = Application.persistentDataPath;  // 非编辑器下的路径
#endif

            // 构建完整的文件夹路径（直接在后面加上传入的folderName）
            string fullPath = basePath + $"/{folderName}/";

            // 确保文件夹路径存在
            EnsureDirectoryExists(fullPath);

            return fullPath;
        }

        // 确保指定路径存在，如果不存在则创建它
        private static void EnsureDirectoryExists(string path)
        {
            // 如果目录不存在，递归创建目录
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
