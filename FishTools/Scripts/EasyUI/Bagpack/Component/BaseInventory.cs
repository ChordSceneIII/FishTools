using UnityEngine;
using System.Collections.Generic;
using System;
///
/// 基于对slot和item实例的检查，而非维护一个仓库数据集，这样虽然更消耗性能但是能更简单和准确反应实际的情况
/// 只有当持久化的时候才进行数据集的维护
///

namespace FishTools.EasyUI
{
    public abstract class BaseInventory<DATA, TYPE> : MonoBehaviour, IBaseInventory where DATA : BaseItemDATA<TYPE> where TYPE : Enum
    {
        #region ####序列化数据
        public List<DATA> itemdatas = new List<DATA>();//临时数据(传递中介)
        public List<string> locked_slots = new List<string>();//锁定的slot
        #endregion

        [Label("Slot预制体")] public GameObject slotPrefab;//slot预制体
        [Label("Slot前缀名")] public string sample_name = "slot_";
        public DictionarySerializable<string, GameObject> slotDic = new DictionarySerializable<string, GameObject>();
        public DictionarySerializable<string, GameObject> SlotDic
        {
            get { return slotDic; }
            set { slotDic = value; }
        }

        //添加格子
        public void AddSlot()
        {
            if (slotPrefab.GetComponent<SlotGroup>() == null)
            {
                DebugF.LogError("预制体没有SlotGroup组件");
                return;
            }
            GameObject new_slot = Instantiate(slotPrefab, transform);

            new_slot.name = sample_name + transform.childCount;

            UpdateSlots();
        }

        //移除格子  ：删除末尾的格子
        public void RemoveSlot()
        {
            if (transform.childCount > 0)
            {
                GameObject last_slot = transform.GetChild(transform.childCount - 1).gameObject;

                if (Application.isPlaying)
                    GameObjectUtility.AfterDestroy(last_slot, () => { UpdateSlots(); });
#if UNITY_EDITOR
                else
                    DestroyImmediate(last_slot);
                UpdateSlots();
#endif
            }
        }

        //更新格子(更新slotDic 和 lockSlotDic)
        public void UpdateSlots()
        {
            slotDic.Clear();
            locked_slots.Clear();

            var slots = GetComponentsInChildren<SlotGroup>();

            foreach (var slot in slots)
            {
                GameObject slotObj = slot.gameObject;

                if (slotDic.ContainsKey(slotObj.name))
                {
                    DebugF.LogWarning("格子名称有重复");
                }

                slotDic[slotObj.name] = slotObj;

                if (slot.IsLocked == true)
                    locked_slots.Add(slotObj.name);
            }

            DebugF.Log("格子更新成功");
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
            DebugF.Log("物品临时数据更新成功");
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
        public (GameObject, T) FindUsedSlot<T>(TYPE type) where T : BaseItem<DATA, TYPE>
        {
            foreach (var slot in slotDic.Values)
            {
                var item = slot.GetComponentInChildren<T>();

                if (item == null)
                {
                    continue;
                }
                //TYPE泛型比较
                if (item.data.Type.type.Equals(type))
                {
                    return (slot, item);
                }
            }
            return (null, default);
        }

        //添加物体
        public T AddItem<T>(DATA data, GameObject slot) where T : BaseItem<DATA, TYPE>
        {
            if (slot != null && data != null)
            {
                var newItemObj = Instantiate(data.Type.GetPrefab(), slot.transform);
                var propItemComponent = newItemObj.GetComponent<T>();
                propItemComponent.data = data;
                propItemComponent.data.slotName = slot.name;
                itemdatas.Add(propItemComponent.data);

                return propItemComponent;
            }
            return null;
        }

        //移除物体
        public void RemoveItem<T>(T item) where T : BaseItem<DATA, TYPE>
        {
            if (item != null)
            {
                if (Application.isPlaying)
                    Destroy(item.gameObject);
#if UNITY_EDITOR
                else
                    DestroyImmediate(item.gameObject);
#endif
            }

        }

        //加载数据并实例化
        public void LoadALLDatas(List<DATA> itemdatas, List<string> locked_index)
        {
            if (itemdatas != null)
            {
                UpdateSlots();
                ClearInventory();

                //解锁所有格子
                foreach (var slot in slotDic.Values)
                {
                    slot.GetComponent<SlotGroup>().IsLocked = false;
                }
                // 锁定需要锁定的格子
                foreach (var index in locked_index)
                {
                    slotDic[index].GetComponent<SlotGroup>().IsLocked = true;
                }

                //实例化
                foreach (var itemdata in itemdatas)
                {
                    //找到对应的格子
                    slotDic.TryGetValue(itemdata.slotName, out var slot);
                    GameObject item = GameObject.Instantiate(itemdata.Type.GetPrefab(), slot.transform);
                    item.GetComponent<BaseItem<DATA, TYPE>>().data = itemdata;
                }

            }
            else
            {
                DebugF.LogWarning("itemdatas is null");
            }
        }

        //更新数据并上传
        public (List<DATA>, List<string>) ReturnALLDatas()
        {
            UpdateSlots();
            UpdateItems();
            return (itemdatas, locked_slots);
        }

        //清除所有物体实例
        public void ClearInventory()
        {
            if (Application.isPlaying)
            {
                itemdatas.Clear();

                foreach (var slot in slotDic.Values)
                {
                    Destroy(slot.GetComponent<SlotGroup>().ItemObj);
                }
            }
#if UNITY_EDITOR         //编辑器下调用的删除物体
            else
            {
                itemdatas.Clear();

                foreach (var slot in slotDic.Values)
                {
                    DestroyImmediate(slot.GetComponent<SlotGroup>().ItemObj);
                }
            }
#endif
        }



    }
}