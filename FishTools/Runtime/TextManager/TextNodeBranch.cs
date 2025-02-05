using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FishTools.SaveTool;

namespace FishTools.TextManager
{
    //文章片段
    [Serializable]
    public class TextNode
    {
        public TextNode(string name, string text)
        {
            this.name = name;
            this.text = text;
        }

        public string text;
        public string name;

        public UnityEvent actionEvent;

        public void ActionEvent_Callback()
        {
            actionEvent?.Invoke();
        }
    };

    [Serializable]
    public class Branch
    {
        public List<TextNode> nodeList = new List<TextNode>();

    }


    //存储路径设置
    public static class BranchSave
    {
        public static string Path
        {
            get
            {
                return Application.streamingAssetsPath + "/TextBranch/";
            }
        }

        public static Branch LoadBranch(string name)
        {
            var branch = SaveSystem.LoadFromJSON<Branch>(Path + name + ".json");
            return branch;
        }

        public static void SaveBranch(Branch branch, string name)
        {
            SaveSystem.SaveToJson(Path + name + ".json", branch);
        }
    }

}
