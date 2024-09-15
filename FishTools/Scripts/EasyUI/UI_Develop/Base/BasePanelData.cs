using UnityEngine;

namespace EasyUI
{
    public abstract class BasePanelData : ScriptableObject
    {
        [SerializeField] internal BasePanel panelPrefab;

        [SerializeField] internal string panelname;

        [SerializeField] internal string path;
        internal void AddToPathDict()
        {
            UIManager_develop.Instance.pathDict.Add(this.panelname, this.path);
        }
        protected void BaseClosePanel()
        {
            UIManager_develop.Instance.ClosePanel(panelname);
        }
        protected void BaseOpenPanel()
        {
            UIManager_develop.Instance.OpenPanel(panelname);
        }
        protected void BaseRepatPanel()
        {
            UIManager_develop.Instance.RepatPanel(panelname);
        }
    }
}