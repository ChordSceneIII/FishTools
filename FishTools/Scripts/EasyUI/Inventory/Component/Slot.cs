using UnityEngine;
using UnityEngine.UI;
using FishTools;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(RectTransform))]
    public class Slot : Button
    {
        [SerializeField] private bool islocked;
        public bool IsLocked
        {
            get
            {
                interactable = !islocked;
                return islocked;
            }
            set
            {
                interactable = !value;
                islocked = value;
            }
        }
        //TODO:锁定另外做处理，不和interactable重叠，interactable已经用来设置focus
        //TODO:制作锁定的选项，为hierarchy添加加载EasyUI组件的快捷方式，比如添加Slot或者添加背包结构
        //TODO:添加存档选择，存档读盘

        [SerializeField, ReadOnly, Label("Item对象")] private GameObject itemObj;
        public GameObject ItemObj
        {
            get
            {
                if (itemObj == null)
                {
                    itemObj = GetComponentInChildren<IBaseItem>()?.gameObject;
                }
                return itemObj;
            }
        }
        public string selectKey = "select";
        [Label("选中事件")] public UnityEvent selectEvent;
        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            selectEvent?.Invoke();
            EventManager.Trigger(selectKey, this);
        }

    }
}
