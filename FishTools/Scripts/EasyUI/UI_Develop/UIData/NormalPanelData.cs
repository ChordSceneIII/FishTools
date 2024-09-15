using UnityEngine;

/// <summary>
///  UI数据
/// </summary>

namespace EasyUI
{
    [CreateAssetMenu(fileName = "uidata", menuName = "EasyUI/DataConfig/NormalPanelData", order = 0)]
    public class NormalPanelData : BasePanelData
    {
        public void ClosePanel()
        {
            BaseClosePanel();
        }
        public void OpenPanel()
        {
            BaseOpenPanel();
        }
        public void RepatPanel()
        {
            BaseRepatPanel();
        }
    }
}
