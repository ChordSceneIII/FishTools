
using UnityEngine;

namespace EasyUI
{
    [CreateAssetMenu(fileName = "uidata", menuName = "EasyUI/DataConfig/FollowPanelData", order = 1)]
    public class FollowPanelData : BasePanelData
    {
        public void OpenPanel(Transform transform)
        {
            //确保引用的对象是新生成的对象
            var panel = UIManager_develop.Instance.OpenNewPanel(panelname);

            if (panel != null)
            {
                if (panel is FollowPanel)
                {
                    (panel as FollowPanel).target = transform;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning("当前面板不是FollowPanel,该方法将无效");
#endif
                }
            }
        }
        public void ClosePanel()
        {
            BaseClosePanel();
        }
    }
}
