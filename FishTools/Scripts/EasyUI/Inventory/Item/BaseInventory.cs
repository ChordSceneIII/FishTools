using UnityEngine;
using FishToolsEditor;
using System.Collections.Generic;
using FishTools;
using System;

///
/// 基于对slot和item实例的检查，而非维护一个仓库数据集，这样虽然更消耗性能但是能更简单和准确反应实际的情况
/// 只有当持久化的时候才进行数据集的维护
///

namespace EasyUI
{
    public abstract class BaseInventory<DATA, TYPE> : MonoBehaviour where DATA : BaseItemDATA<TYPE> where TYPE : Enum
    {
        public List<DATA> itemdatas;//临时数据(传递中介)
        public DictionarySerializable<string, GameObject> SlotDic = new DictionarySerializable<string, GameObject>();

        //更新格子
        public void UpdateSlots()
        {
            SlotDic.Clear();
            foreach (Transform child in transform)
            {
                GameObject slot = child.gameObject;

                if (SlotDic.ContainsKey(slot.name))
                {
                    DebugEditor.LogWarning("格子名称有重复");
                }

                SlotDic[slot.name] = slot;
            }
        }

        //更新数据
        public void UpdateItems()
        {
            itemdatas.Clear();
            foreach (Transform child in transform)
            {
                var item = child.GetComponentInChildren<BaseItem<DATA, TYPE>>();

                if (item != null)
                {
                    item.data.slotName = child.name;
                    itemdatas.Add(item.data);
                }
            }
            DebugEditor.Log("更新成功");
        }

        //寻找空格子
        public GameObject FindEmptySlot()
        {
            foreach (Transform child in transform)
            {
                var slot = child.gameObject;
                //检查格子是否为空
                if (slot.GetComponentInChildren<BaseItem<DATA, TYPE>>() == null)
                {
                    return slot;
                }
            }
            return null;
        }
        //寻找有某种物体的格子（限第一个找到的）
        public (GameObject, BaseItem<DATA, TYPE>) FindUsedSlot(Enum type)
        {
            foreach (Transform child in transform)
            {
                var slot = child.gameObject;
                var item = slot.GetComponentInChildren<BaseItem<DATA, TYPE>>();
                //TYPE泛型比较
                if (item.data.Type.value.Equals(type))
                {
                    return (slot, item);
                }
            }
            return (null, null);
        }

        //物体的添加和移除方法自定，因为不同物品的添加和移除逻辑不同
        public abstract void AddItem(DATA data);
        public abstract void RemoveItem(DATA data);

        //加载数据并实例化
        public void LoadALLDatas(List<DATA> itemdatas)
        {
            ClearItemAndData();
            UpdateSlots();

            //实例化
            foreach (var itemdata in itemdatas)
            {
                //找到对应的格子
                SlotDic.TryGetValue(itemdata.slotName, out var slot);
                GameObject item = GameObject.Instantiate(itemdata.Type.GetObj(), slot.transform);
                item.GetComponent<BaseItem<DATA, TYPE>>().data = itemdata;
            }
        }

        //更新数据并上传
        public List<DATA> ReturnALLDatas()
        {
            UpdateItems();
            return itemdatas;
        }

        //清除所有实例以及缓存数据
        public void ClearItemAndData()
        {
            itemdatas.Clear();
            foreach (Transform slot in transform)
            {
                foreach (Transform item in slot)
                {
                    GameObject.Destroy(item?.gameObject);
                }
            }
        }
    }

}
