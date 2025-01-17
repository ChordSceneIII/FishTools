using System;
using FishTools.EasyUI;
using UnityEngine;
using UnityEngine.UI;

namespace FishToolsDEMO
{
    [Serializable]
    public class TestItemDATA : BaseItemDATA<TestTypeEnum>
    {
        public TestItemDATA(int _count, TestTypeEnum _typevalue)
        {
            Count = _count;
            type = _typevalue;
        }
    }

    public class TestItem : BaseItem<TestItemDATA, TestTypeEnum>
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