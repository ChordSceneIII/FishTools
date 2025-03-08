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
            get => islocked;
            set
            {
                LockSlot(value);
            }
        }
        [SerializeField, ReadOnly, Label("Item对象")] private GameObject itemObj;
        private IBaseItem iitem;
        public IBaseItem IItem
        {
            get
            {
                if (FishUtility.IsNull(iitem))
                {
                    iitem = GetComponentInChildren<IBaseItem>();
                    itemObj = iitem?.gameObject;
                }
                return iitem;
            }
        }
        [SerializeField] private Image lockedImage;
        [SerializeField] private Image selectImage;
        public string selectKey = "select";
        public string submitKey = "submit";
        protected override void Awake()
        {
            base.Awake();
        }

        private void LockSlot(bool isLock)
        {
            if (isLock)
            {
                islocked = true;
                lockedImage?.gameObject.SetActive(true);
            }
            else
            {
                islocked = false;
                lockedImage?.gameObject.SetActive(false);
            }
        }
        //加锁后不能选择，可以利用lockedImage覆盖

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            selectImage?.gameObject.SetActive(true);
            EventManager.Trigger<Slot>(selectKey, this);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            selectImage?.gameObject.SetActive(false);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            EventManager.Trigger<Slot>(submitKey, this);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            EventManager.Trigger<Slot>(submitKey, this);
        }
    }
}
