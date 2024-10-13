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
        public List<DATA> itemdatas = new List<DATA>();//临时数据(传递中介)
        public DictionarySerializable<string, GameObject> slotDic = new DictionarySerializable<string, GameObject>();

        protected virtual void OnEnable()
        {
            UpdateSlots();
            UpdateItems();
        }

        //更新格子
        public void UpdateSlots()
        {
            slotDic.Clear();

            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                GameObject slot = child.gameObject;
                if (child.name.Contains("slot"))
                {
                    if (slotDic.ContainsKey(slot.name))
                    {
                        DebugEditor.LogWarning("格子名称有重复");
                        continue;
                    }
                    slotDic[slot.name] = slot;
                }
            }
        }

        //更新数据
        public void UpdateItems()
        {
            itemdatas.Clear();

            BaseItem<DATA, TYPE>[] items = GetComponentsInChildren<BaseItem<DATA, TYPE>>();
            foreach (var item in items)
            {
                item.data.slotName = item.transform.parent.name;
                itemdatas.Add(item.data);
            }
            DebugEditor.Log("物品临时数据更新成功");
        }


        //寻找空格子
        public GameObject FindEmptySlot()
        {
            foreach (var slot in slotDic.Values)
            {
                //检查格子是否为空
                if (slot.GetComponentInChildren<BaseItem<DATA, TYPE>>() == null)
                {
                    return slot;
                }
            }
            return null;
        }
        //寻找有某种物体的格子（限第一个找到的）
        public (GameObject, T) FindUsedSlot<T>(Enum type) where T : BaseItem<DATA, TYPE>
        {
            foreach (var slot in slotDic.Values)
            {
                var item = slot.GetComponentInChildren<T>();

                if (item == null)
                {
                    continue;
                }
                //TYPE泛型比较
                if (item.data.Type.value.Equals(type))
                {
                    return (slot, item);
                }
            }
            return (null, default);
        }

        //物体的添加和移除方法自定，因为不同物品的添加和移除逻辑不同
        public abstract void AddItem(DATA data);
        public abstract void RemoveItem(DATA data);

        //加载数据并实例化
        public void LoadALLDatas(List<DATA> itemdatas)
        {
            UpdateSlots();
            ClearInventory();

            //实例化
            foreach (var itemdata in itemdatas)
            {
                //找到对应的格子
                slotDic.TryGetValue(itemdata.slotName, out var slot);
                GameObject item = GameObject.Instantiate(itemdata.Type.GetPrefab(), slot.transform);
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
        public void ClearInventory()
        {
            itemdatas.Clear();

            foreach (var slot in slotDic.Values)
            {
                foreach (Transform child in slot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

}
