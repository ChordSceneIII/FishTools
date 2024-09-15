/// <summary>
/// 数据文件的基类
/// </summary>

namespace FishTools
{
    public interface BaseData
    {
        public void LoadData(string path);
        public void SaveData(string path);

        ///
        /// 使用例
        ///
        // public override void LoadData(string path)
        // {
        //     PlayerData data = SaveSystem.LoadFromJSON<PlayerData>(path);
        //     CurHP = data.CurHP;
        //     MaxHP = data.MaxHP;
        // }

        // public override void SaveData(string path)
        // {
        //     SaveSystem.SaveToJson(path, this);
        // }
    }
}
