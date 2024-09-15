using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector] public Transform originalParent;
        [HideInInspector] public Vector2 oringalpos;
        [HideInInspector] public Inventory belongToInventory;//隶属背包
        [HideInInspector] public Inventory cureentPointInventory;//当前指向背包

        public void Start()
        {
            belongToInventory = GetComponentInParent<Inventory>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (belongToInventory.lockDragItem == false)
            {

                originalParent = this.transform.parent.transform;
                oringalpos = this.transform.position;

                transform.SetParent(GetComponentInParent<Canvas>().transform, false);//显示在最上层
                transform.position = eventData.position;
                GetComponent<CanvasGroup>().blocksRaycasts = false;//取消阻挡射线
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (belongToInventory.lockDragItem == false)
            {
                transform.position = eventData.position;

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (belongToInventory.lockDragItem == false)
            {
                GameObject temp = eventData.pointerCurrentRaycast.gameObject;
                bool canDrag = true;

                if (temp != null)
                {
                    cureentPointInventory = temp.GetComponentInParent<Inventory>();

                    //如果移动的背包不同
                    if (cureentPointInventory != null && belongToInventory != cureentPointInventory)
                    {
                        canDrag = cureentPointInventory.isLocked == false && belongToInventory.isLocked == false;
                    }

                    //如果在同一个背包内移动
                    if (cureentPointInventory == belongToInventory)
                    {
                        canDrag = true;
                    }
                }

                /// <summary>
                /// 如果有需要可以把通过name判断的代码改成通过tag判断
                /// </summary>

                //扩充射线探测范围至整个槽位而不是Item的渲染范围
                if (temp != null && canDrag && temp.transform.Find("item") != null)
                {
                    temp = temp.transform.Find("item").gameObject;
                }

                // 交换物体
                if (temp != null && canDrag && temp.name == "item")
                {
                    this.transform.SetParent(temp.transform.parent.transform, false);
                    this.transform.position = temp.transform.position;

                    temp.transform.SetParent(originalParent);
                    temp.transform.position = oringalpos;

                }
                //空槽位移动
                else if (temp != null && canDrag && temp.name.StartsWith("slot"))
                {
                    this.transform.SetParent(temp.transform, false);
                    this.transform.position = temp.transform.position;

                }
                //复位
                else
                {
                    this.transform.SetParent(originalParent);
                    this.transform.position = oringalpos;
                }

                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }
}
