using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    public class CommonHandler : BaseHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IDragHandler
    {
        [Flags]
        public enum EventType
        {
            None = 0,
            Hover = 1 << 0,
            OutHover = 1 << 1,
            ClickInside = 1 << 2,
            ClickOutside = 1 << 3,
            Enter = 1 << 4,
            Exit = 1 << 5,
            ChildEnter = 1 << 6,
            ChildExit = 1 << 7,
            Drag = 1 << 8,
            OnInteractable = 1 << 9,
            OffInteractable = 1 << 10,
        }

        public EventType eventType = EventType.None;

        [ConditionalField("eventType", EventType.Hover)] public UnityEvent hoverEvent;
        [ConditionalField("eventType", EventType.OutHover)] public UnityEvent outHoverEvent;
        [ConditionalField("eventType", EventType.ClickInside)] public UnityEvent clickInsideEvent;
        [ConditionalField("eventType", EventType.ClickOutside)] public UnityEvent clickOutsideEvent;
        [ConditionalField("eventType", EventType.Enter)] public UnityEvent enterEvent;
        [ConditionalField("eventType", EventType.Exit)] public UnityEvent exitEvent;
        [ConditionalField("eventType", EventType.ChildEnter)] public UnityEvent childEnterEvent;
        [ConditionalField("eventType", EventType.ChildExit)] public UnityEvent childExitEvent;
        [ConditionalField("eventType", EventType.OnInteractable)] public UnityEvent onInteractableEvent;
        [ConditionalField("eventType", EventType.OffInteractable)] public UnityEvent offInteractableEvent;

        private bool isInside = false;
        private List<Transform> childs;
        private GameObject lastChild;//记录上一选择的子项
        private bool lastInteractable;//记录上一交互状态

        protected override void Start()
        {
            base.Start();
            //获取子项组件
            childs = new List<Transform>();
            childs = GetComponentsInChildren<Transform>(true).ToList();
            lastInteractable = !interactable;
        }

        protected override void Update()
        {
            base.Update();

            if (interactable)
            {
                if ((eventType & EventType.Hover) != 0 && isInside)
                {
                    hoverEvent?.Invoke();
                }

                if ((eventType & EventType.OutHover) != 0 && !isInside)
                {
                    outHoverEvent?.Invoke();
                }

                if ((eventType & EventType.ClickInside) != 0 && isInside && Input.GetMouseButtonDown(0))
                {
                    clickInsideEvent?.Invoke();
                }

                if ((eventType & EventType.ClickOutside) != 0 && !isInside && Input.GetMouseButtonDown(0))
                {
                    clickOutsideEvent?.Invoke();
                }

                if (((eventType & EventType.ChildEnter) != 0) || ((eventType & EventType.ChildExit) != 0))
                {
                    //记录上一项
                    if (EventSystem.current.currentSelectedGameObject != lastChild)
                    {
                        lastChild = EventSystem.current.currentSelectedGameObject;

                        //选择对象包括子项时
                        if (lastChild != null && childs.Contains(lastChild.transform))
                        {
                            if ((eventType & EventType.ChildEnter) != 0)
                            {
                                childEnterEvent?.Invoke();
                            }
                        }
                        //离开时
                        else if ((eventType & EventType.ChildExit) != 0)
                        {
                            childExitEvent?.Invoke();
                        }

                    }
                }
            }

            if (interactable != lastInteractable)
            {
                lastInteractable = interactable;

                if (interactable)
                {
                    if ((eventType & EventType.OnInteractable) != 0) onInteractableEvent?.Invoke();
                }
                else
                {
                    if ((eventType & EventType.OffInteractable) != 0) offInteractableEvent?.Invoke();
                }
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (interactable)
            {
                isInside = true;
                if ((eventType & EventType.Enter) != 0)
                {
                    enterEvent?.Invoke();
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (interactable)
            {
                isInside = false;
                if ((eventType & EventType.Exit) != 0)
                {
                    exitEvent?.Invoke();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (interactable && (eventType & EventType.Drag) != 0)
            {
                transform.position += ScreenToWorldDelta(eventData.delta, Camera.main);
            }
        }
        private Vector3 ScreenToWorldDelta(Vector2 screenDelta, Camera camera)
        {
            if (camera == null)
            {
                Debug.LogError("摄像机未设置！");
                return Vector3.zero;
            }

            // 将屏幕坐标增量转换为世界坐标增量
            Vector3 screenPos1 = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            Vector3 screenPos2 = camera.ScreenToWorldPoint(new Vector3(screenDelta.x, screenDelta.y, camera.nearClipPlane));

            return screenPos2 - screenPos1;
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (interactable)
            {
                if ((eventType & EventType.Enter) != 0)
                {
                    enterEvent?.Invoke();
                }
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (interactable)
            {
                if ((eventType & EventType.Exit) != 0)
                {
                    exitEvent?.Invoke();
                }
            }
        }
    }
}