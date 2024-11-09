using UnityEngine;
/// <summary>
/// 跟随面板
/// </summary>

namespace FishTools.EasyUI
{
    [CreateAssetMenu(fileName = "follow", menuName = "EayUI/Follow", order = 1)]
    public class FollowPanelData : BasePanelData<FollowPanel>
    {
        public void Open(Transform transform)
        {
            var panel = UIManager_develop.Instance.OpenPanel(panelname);
            panel.gameObject.SetActive(false);
            if (panel is FollowPanel)
            {
                (panel as FollowPanel).SetTarget(transform);
            }
            panel.gameObject.SetActive(true);
        }
        public void Close()
        {
            UIManager_develop.Instance.ClosePanel(panelname);
        }
        public void Repeat()
        {
            UIManager_develop.Instance.RepeatPanel(panelname);
        }

        public static FollowPanel Open_Pool(string panelname, string panelIndex, Transform transform)
        {
            FollowPanel panel = null;

            //如果panelIndex存在，则打开panelIndex
            if (UIManager_pool.Instance.ContainsPanel(panelIndex))
            {
                UIManager_pool.Instance.OpenPanel(panelIndex);
            }
            //否则新建一个panel
            else
            {
                panel = UIManager_pool.Instance.GetNewPanel(panelname, panelIndex, false) as FollowPanel;
                panel.SetTarget(transform);
                panel.gameObject.SetActive(true);
            }

            return panel;
        }

        public static void Close_Pool(string panelIndex)
        {
            UIManager_pool.Instance.ClosePanel(panelIndex);
        }
    }
}
