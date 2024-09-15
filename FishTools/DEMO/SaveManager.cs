using System.Collections;
using System.Collections.Generic;
using System.IO;
using Codice.Client.Common;
using UnityEngine;

/// <summary>
/// 存档管理中心：管理存档位置，存档数量，存档文件名
/// 以及BaseSave派生类的引用
/// </summary>

namespace FishTools
{
    public class SaveManager : BaseSingletonMono<SaveManager>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        [SerializeField] private string currentSave;//当前存档
        public static string suffix = ".sav";//存档后缀
        public static string GetPath(string filename)
        {
            if (filename != null)
            {
                return Path.Combine(Application.persistentDataPath, filename + suffix);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"未设置存档文件名 ");
#endif
                return null;
            }
        }

        public List<string> saves = new List<string>();//所有存档

#if UNITY_EDITOR
        //检查path路径找到所有存档
        [ContextMenu("查找存档")]
        public void CheckPath()
        {
            saves.Clear();
            string[] files = Directory.GetFiles(Application.persistentDataPath, "*" + suffix);
            foreach (string file in files)
            {
                saves.Add(Path.GetFileNameWithoutExtension(file));
            }
            Debug.Log($"找到{files.Length}个存档");
        }
#endif

        public void Load(string save)
        {
            if (save != null)
            {
                // 加载玩家数据
                PlayerEntity.Instance.datas.LoadData(GetPath(save));
                currentSave = save;
            }
        }

        public void Save(string save)
        {
            if (save != null)
            {
                // 保存玩家数据
                PlayerEntity.Instance.datas.SaveData(GetPath(save));
            }
        }
    }
}
