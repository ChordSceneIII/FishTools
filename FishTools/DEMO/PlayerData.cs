using System;
using UnityEngine;

namespace FishTools
{

    [Serializable]
    public class PlayerData : ISave
    {
        [Tooltip("当前生命")]
        [SerializeField]
        private int curHP;
        public int CurHP
        {
            get { return curHP; }
            set { curHP = Mathf.Clamp(value, 0, maxHP); }
        }

        [Tooltip("生命上限")]
        [SerializeField]
        private int maxHP;
        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }

        public void LoadData(string path)
        {
            PlayerData data = SaveSystem.LoadFromJSON<PlayerData>(path);
            MaxHP = data.MaxHP;
            CurHP = data.CurHP;
        }

        public void SaveData(string path)
        {
            SaveSystem.SaveToJson(path, this);
        }
    }
}