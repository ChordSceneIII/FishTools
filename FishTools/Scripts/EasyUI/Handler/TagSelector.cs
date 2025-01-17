using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FishTools.EasyUI
{
    /// <summary>
    /// 按钮组选择器
    /// </summary>
    public class TagSelector : BaseHandler, IPointerClickHandler, ISubmitHandler, ISelectHandler
    {
        public enum SelectionMode
        {
            ClickOrSubmit, // 必须点击或 Submit 后选择
            OnSelect       // Select 到时就选择
        }

        [Label("遮罩")] public GameObject imageMask;
        [Label("分组标签 ")] public string groupTag = "group1";
        [Label("选择模式")] public SelectionMode selectMode = SelectionMode.ClickOrSubmit;
        [Label("选择事件")] public UnityEvent onSelect;
        [Label("取消选择事件")] public UnityEvent onCancel;
        public static Dictionary<string, List<TagSelector>> groupDict = new Dictionary<string, List<TagSelector>>();

        private void OnEnable()
        {
            if (!groupDict.ContainsKey(groupTag))
            {
                groupDict.Add(groupTag, new List<TagSelector>());
            }
            groupDict[groupTag].Add(this);
        }
        private void OnDisable()
        {
            if (groupDict.ContainsKey(groupTag))
            {
                groupDict[groupTag].Remove(this);
            }
        }

        public void Deselect()
        {
            imageMask.gameObject.SetActive(false);
            onCancel.Invoke();
        }

        public void Select()
        {
            imageMask.gameObject.SetActive(true);
            onSelect.Invoke();

            foreach (var item in groupDict[groupTag])
            {
                if (item != this)
                {
                    item.Deselect();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
                Select();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (interactable)
                Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (interactable)
                if (selectMode == SelectionMode.OnSelect)
                {
                    Select();
                }
        }
    }
}