using UnityEngine;


namespace EasyUI
{
    [CreateAssetMenu(fileName = "default", menuName = "EayUI/Default", order = 1)]
    public class DefaultPanelData : BasePanelData<DefaultPanel>
    {
        public void Open()
        {
            BaseDeOpen();
        }

        public void Repeat()
        {
            BaseDeRepat();
        }

        public void Close()
        {
            BaseDeClose();
        }

        public static int OpenNew(string panelname)
        {
            return UIManager_pool.Instance.GetNewPanel(panelname).Item2;
        }
        public static void Open(int refID)
        {
            UIManager_pool.Instance?.OpenPanel(refID);
        }
        public static void RepatPanel(int refID)
        {
            UIManager_pool.Instance?.RepatPanel(refID);
        }
        public static void Close(int refID)
        {
            UIManager_pool.Instance?.ClosePanel(refID);
        }
    }
}
