using UnityEngine;
using UnityEngine.EventSystems;
using FishToolsEditor;

/// <summary>
/// 物品拖拽堆叠的控制逻辑 : 在这里可以修改控制堆叠逻辑
/// </summary>

namespace EasyUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [ReadOnly] public Transform originalParent;
        [ReadOnly] public Vector2 oringalpos;
        [ReadOnly] public InventoryControl belongToInventory;//隶属背包
        [ReadOnly] public InventoryControl cureentPointInventory;//当前指向背包

        private void Awake()
        {
            gameObject.name = "item";
        }
        private void Start()
        {
            belongToInventory = GetComponentInParent<InventoryControl>();
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
                GameObject target = eventData.pointerCurrentRaycast.gameObject;
                bool canDrag = true;

                if (target != null)
                {
                    cureentPointInventory = target.GetComponentInParent<InventoryControl>();

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

                //扩充射线探测范围至整个槽位而不是Item的渲染范围
                if (target != null && canDrag && target.transform.Find("item") != null)
                {
                    target = target.transform.Find("item").gameObject;
                }

                // 交换物体
                if (target != null && canDrag && target.name == "item")
                {
                    // var targetInfo = target.GetComponent<IDataToView>();
                    // var thisInfo = GetComponent<IDataToView>();

                    // if (targetInfo.ITypeValue == thisInfo.ITypeValue)
                    // {
                    //     //假如类型相同且相加不大于最大上限即可叠加
                    // }

                    this.transform.SetParent(target.transform.parent.transform, false);
                    this.transform.position = target.transform.position;

                    target.transform.SetParent(originalParent);
                    target.transform.position = oringalpos;
                }

                //空槽位移动
                else if (target != null && canDrag && target.name.Contains("slot"))
                {
                    this.transform.SetParent(target.transform, false);
                    this.transform.position = target.transform.position;

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
