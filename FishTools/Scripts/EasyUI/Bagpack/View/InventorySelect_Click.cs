using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static FishTools.EasyUI.ViewUtils;
using System.Linq;


namespace FishTools.EasyUI
{
    public sealed class InventorySelect_Click : View
    {
        [Label("指向Slot"), ReadOnly] public SlotGroup target_slot = null;//当前选择的格子
        [Label("指向Item"), ReadOnly] public GameObject target_itemObj = null;
        [Label("排列行数")] public int columnCount = 5;
        [Label("确认Event")] public UnityEvent submitEvent;//确认

        public List<SlotGroup> Slots
        {
            get
            {
                return transform.GetComponentsInChildren<SlotGroup>().ToList();
            }
        }

        private void OnEnable()
        {
            if (target_slot == null)
            {
                target_slot = transform.GetComponentInChildren<SlotGroup>();
                target_itemObj = target_slot.ItemObj;
                target_slot.IsSelected = true;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Select(false);
                var result = SelectByClick();
                target_slot = result.Item1;
                target_itemObj = result.Item2;
                Select(true);
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Select(false);
                var result = SelectByClick();
                target_slot = result.Item1;
                target_itemObj = result.Item2;
                Select(true);

                submitEvent?.Invoke();
                return;
            }

            GetDirection(out var direction);

            if (direction != Direction.none)
            {
                Select(false);
                var result = SelectByButton(direction);
                target_slot = result.Item1;
                target_itemObj = result.Item2;
                Select(true);
                return;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                submitEvent?.Invoke();
            }

        }

        public void Select(bool value)
        {
            if (target_slot != null)
            {
                target_slot.IsSelected = value;
            }
        }
        public void Lock(bool value)
        {
            if (target_slot != null)
                target_slot.IsLocked = value;
        }

        public (SlotGroup, GameObject) SelectByClick()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            // 获取点击的UI对象
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                var slot = result.gameObject?.GetComponent<SlotGroup>();

                //如果目标是本背包下的slot
                if (Slots.Contains(slot))
                {
                    var item = slot.ItemObj;
                    return (slot, item);
                }
            }
            return (target_slot, target_itemObj);
        }
        //TOOD :修改掉吧，不对出战面板做修改,Slot锁定为仅限子对象这里没问题。但是我们需要设置一个开关用于是否能选择
        //TODO ;然后就是 对应的选择逻辑，比如打开某个小面板

        public (SlotGroup, GameObject) SelectByButton(Direction direction)
        {
            int index = 0;

            if (target_slot != null)
            {
                index = target_slot.transform.GetSiblingIndex();
            }
            else
            {
                return (target_slot, target_itemObj);
            }

            switch (direction)
            {
                case Direction.up:
                    {
                        if (index >= columnCount)
                            index -= columnCount;
                    }
                    break;
                case Direction.down:
                    {
                        if (index + columnCount < transform.childCount)
                            index += columnCount;
                    }
                    break;
                case Direction.left:
                    {
                        if (index >= 1)
                            index -= 1;
                    }
                    break;
                case Direction.right:
                    {
                        if (index + 1 < transform.childCount)
                            index += 1;
                    }
                    break;
            }
            var slot = transform.GetChild(index).GetComponent<SlotGroup>();
            var item = slot.ItemObj;
            return (slot, item);
        }

    }

}
