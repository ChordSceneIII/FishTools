using System;
using FishTools.EasyUI;
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.Tests
{
    [Serializable]
    public class TestItemDATA : BaseItemDATA<TestType>
    {
        public TestItemDATA(int _count, TestType _typevalue)
        {
            Count = _count;
            type = _typevalue;
        }
    }

    public class TestItem : BaseItem<TestItemDATA, TestType>
    {
        private Text textCount;
        public Text TextCount
        {
            get
            {
                if (textCount == null)
                    textCount = transform.parent.GetComponentInChildren<Text>();
                return textCount;
            }
        }

        public override void AddCount(int count)
        {
            data.Count += count;
            if (data.Count <= 0)
            {
                if (Application.isPlaying)
                    Destroy(this.gameObject);
            }

            TextCount.text = data.Count.ToString();
        }
    }
}