using FishToolsEditor;
using UnityEngine;

/// <summary>
/// 跟随面板
/// </summary>

namespace EasyUI
{
    [CreateAssetMenu(fileName = "follow", menuName = "EayUI/Follow", order = 1)]
    public class FollowPanelData : BasePanelData<FollowPanel>
    {
        public void Open(Transform transform)
        {
            //确保引用的对象是新生成的对象
            var panel = UIManager_develop.Instance.OpenNewPanel(panelname);

            if (panel != null)
            {
                if (panel is FollowPanel)
                {
                    //更新面板的追踪目标
                    (panel as FollowPanel).TargetTrans = transform;
                }
                else
                {
                    DebugEditor.LogWarning("当前面板不是FollowPanel,该方法将无效");
                }
            }
        }

        public void Close()
        {
            BaseDeClose();
        }

        //静态方法，只写pool的调用
        public static void Open(int refID)
        {
            UIManager_pool.Instance.OpenPanel(refID);
        }

        public static int OpenNew(Transform transform, string panelname)
        {
            var panelTuple = UIManager_pool.Instance.GetNewPanel(panelname);
            var panel = panelTuple.Item1;
            var panelID = panelTuple.Item2;

            if (panel != null)
            {
                //更新面板的追踪目标
                (panel as FollowPanel).TargetTrans = transform;
                return panelID;
            }
            else
            {
                DebugEditor.LogWarning("当前面板不是FollowPanel,该方法将无效");
                return default;
            }
        }

        public static void Close(int panelID)
        {
            UIManager_pool.Instance.ClosePanel(panelID);
        }
    }
}
