using UnityEngine;
/// <summary>
/// 默认面板
/// </summary>

namespace FishTools.EasyUI
{
    [CreateAssetMenu(fileName = "default", menuName = "EayUI/Default", order = 1)]
    public class DefaultPanelData : BasePanelData<DefaultPanel>
    {
        [Label("拖拽")] public bool canDrag;
        [Label("聚焦")] public bool focus;


        /// <summary>
        /// destroy 勾选代表在游戏中采用Destroy和Instantiate的方式打开面板，不勾选则代表使用SetActive的方式打开面板
        /// </summary>

        public void Open(bool destroy)
        {
            if (destroy)
                UIManager_develop.Instance.GetNewPanel(panelname);
            else
                UIManager_develop.Instance.OpenPanel(panelname);
        }
        public void Close(bool destroy)
        {
            if (destroy)
                UIManager_develop.Instance.DestroyPanel(panelname);
            else
                UIManager_develop.Instance.ClosePanel(panelname);
        }
        public void Repeat(bool destroy)
        {
            if (destroy)
                UIManager_develop.Instance.RepeatNewPanel(panelname);
            else
                UIManager_develop.Instance.RepeatPanel(panelname);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (basepanel.canDrag != canDrag)
                basepanel.canDrag = canDrag;

            if (basepanel.focus != focus)
                basepanel.focus = focus;
        }
#endif
    }
}
