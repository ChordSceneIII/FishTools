using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
///
/// 基于对slot和item实例的检查，而非维护一个仓库数据集，这样虽然更消耗性能但是能更简单和准确反应实际的情况
/// 只有当持久化的时候才进行数据集的维护
///

namespace FishTools.EasyUI
{
    /// <summary>
    /// Inventory序列化存储数据
    /// </summary>
    [Serializable]
    public struct InventoryData<DATA>
    {
        public int slot_count; //背包大小
        public List<DATA> itemdatas; // 物品数据
        public List<int> locked_Index; // 锁定的格子的序列信息

        public void Clear()
        {
            slot_count = 0;
            itemdatas = new List<DATA>();
            locked_Index = new List<int>();
        }

        public void Set(int slot_count, List<DATA> itemdatas, List<int> locked_Index)
        {
            this.slot_count = slot_count;
            this.itemdatas = itemdatas;
            this.locked_Index = locked_Index;
        }
    }

    public abstract class BaseInventory<DATA, ENUM, TYPE> : MonoBehaviour, IBaseInventory where DATA : BaseItemDATA<ENUM> where ENUM : Enum where TYPE : BaseItemType<ENUM>
    {

        #region #### 属性设置
        [Label("ItemType配置器")] public TYPE typeConfig;
        [Label("Slot预制体")] public GameObject slotPrefab;
        [Label("Slot前缀名")] public string sample_name = "slot_";

        /// <summary>
        /// slot映射列表
        /// </summary>
        private List<Slot> slotList = new List<Slot>();
        public List<Slot> SlotList => slotList;
        #endregion

        #region ####序列化数据
        [Label("临时数据"), SerializeField] public InventoryData<DATA> data;
        #endregion
        protected virtual void OnEnable()
        {
            UpdateBag();
        }

        /// <summary>
        /// 添加一个格子
        /// </summary>
        public void AddSlot()
        {
            if (slotPrefab == null)
            {
                DebugF.LogError("请先设置slot预制体样式");
            }

            GameObject new_slot = Instantiate(slotPrefab, transform);
            Slot slot = new_slot.GetComponent<Slot>();

            if (slot == null)
            {
                DebugF.LogError("预制体没有SlotGroup组件");
                return;
            }

            slotList.Add(slot);
            data.slot_count = slotList.Count;

            //修改slot名字
            new_slot.name = sample_name + slotList.IndexOf(slot);

        }

        /// <summary>
        /// 从末尾队列移除一个格子
        /// </summary>
        public void RemoveSlot()
        {
            if (transform.childCount > 0)
            {
                Slot last_slot = slotList[slotList.Count - 1];
                var item = last_slot.GetComponentInChildren<BaseItem<DATA, ENUM>>();

                if (Application.isPlaying)
                {
                    slotList.Remove(last_slot);
                    //如果slot上有物体 则从临时数据中移除
                    if (item != null)
                    {
                        data.itemdatas.Remove(item.data);
                    }

                    Destroy(last_slot.gameObject);
                }
#if UNITY_EDITOR
                else
                {
                    slotList.Remove(last_slot);
                    if (item != null)
                    {
                        data.itemdatas.Remove(item.data);
                    }
                    DestroyImmediate(last_slot.gameObject);
                }
#endif
                data.slot_count = slotList.Count;
            }
        }

        /// <summary>
        /// 更新当前格子的映射列表
        /// </summary>
        public void UpdateSlotList()
        {
            slotList.Clear();
            slotList = GetComponentsInChildren<Slot>().ToList();
        }

        /// <summary>
        /// 更新当前的背包数据
        /// </summary>
        public void UpdateBag()
        {
            data.Clear();

            //更新位置映射
            UpdateSlotList();

            //更新背包大小
            data.slot_count = slotList.Count;

            foreach (var slot in slotList)
            {
                if (slot == null)
                {
                    DebugF.LogError("SlotGroup组件丢失");
                    return;
                }

                //更新锁定信息
                if (slot.IsLocked == true)
                {
                    data.locked_Index.Add(slotList.IndexOf(slot));
                }

                //更新物品数据
                var item = slot.GetComponentInChildren<BaseItem<DATA, ENUM>>();
                if (item != null)
                {
                    item.data.slotIndex = slotList.IndexOf(slot);
                    data.itemdatas.Add(item.data);
                }
            }

            // DebugF.Log("物品临时数据更新成功");
        }

        /// <summary>
        /// 寻找未被锁定的空背包格
        /// </summary>
        public Slot FindEmptySlot()
        {
            foreach (var slot in slotList)
            {
                var item = slot.ItemObj;
                //如果格子为空且没有被锁定 则返回
                if (item == null && slot.IsLocked == false)
                {
                    DebugF.Log($"找到格子序号为{slotList.IndexOf(slot)}");
                    return slot;
                }
            }
            DebugF.Log($"找不到可用格子");
            return null;
        }

        /// <summary>
        /// 判断背包是否已满
        /// </summary>
        public bool IsFull()
        {
            foreach (var slot in slotList)
            {
                var item = slot.ItemObj;
                if (item != null)
                {
                    return false;
                }
            }

            DebugF.LogWarning("背包已满");
            return true;
        }

        /// <summary>
        /// 寻找带有特定类型Item的格子
        /// </summary>
        public (Slot, T) FindUsedSlot<T>(ENUM type) where T : BaseItem<DATA, ENUM>
        {
            foreach (var slot in slotList)
            {
                var item = slot.GetComponentInChildren<T>();

                if (item == null || slot.IsLocked == true)
                {
                    // 跳过空格子或被锁定的格子
                    continue;
                }
                //TYPE泛型比较
                if (item.data.type.Equals(type))
                {
                    return (slot, item);
                }
            }
            return (null, default);
        }

        /// <summary>
        /// 通过DATA数据 添加物体实例，可以预先指定槽位位置
        /// </summary>
        public T AddItem<T>(DATA data, int slotIndex, bool iscopy = true) where T : BaseItem<DATA, ENUM>
        {
            if (typeConfig == null)
            {
                DebugF.LogError("没有配置typeConfig");
                return null;
            }

            if (data == null)
            {
                DebugF.LogError("不能导入空数据");
                return null;
            }
            if (!typeConfig.TryGetPrefab(data.type, out var prefab))
            {
                DebugF.LogError("预制体不存在或者未正确配置,请检查tyepeConfig");
                return null;
            }

            if (slotIndex < 0 || slotIndex >= slotList.Count)
            {
                DebugF.Log("索引超出现有格子范围");
                return null;
            }

            var slot = slotList[slotIndex];
            var new_obj = Instantiate(prefab, slot.transform);
            var item = new_obj.GetComponent<T>();

            if (iscopy)
                item.data = FishUtility.DeepCopy(data);//拷贝数据到新的对象上
            else
                item.data = data;//引用数据到新的对象上

            item.data.slotIndex = slotIndex; //更新索引信息
            item.AddCount(0); //更新AddCount响应
            this.data.itemdatas.Add(item.data);//更新数据到临时列表中

            return item;
        }

        /// <summary>
        /// (通过SlotGroup引用)通过DATA数据 添加物体实例，可以预先指定槽位引用
        /// </summary>
        public T AddItem<T>(DATA data, Slot slotGroup, bool iscopy = true) where T : BaseItem<DATA, ENUM>
        {
            if (slotList.Contains(slotGroup) == false)
            {
                DebugF.LogError($"{slotGroup}不在{slotList}中");
                return null;
            }

            var _index = slotList.IndexOf(slotGroup);

            return AddItem<T>(data, _index, iscopy);
        }
        /// <summary>
        /// 移除物体,通过指定引用指定删除
        /// </summary>
        public void RemoveItem<T>(T item) where T : BaseItem<DATA, ENUM>
        {
            if (item == null)
            {
                DebugF.LogError("无法移除空对象");
                return;
            }
            if (this.data.itemdatas.Contains(item.data) == false)
            {
                DebugF.LogWarning($"itemdata[{item.gameObject}] 不在当前背包中");
                return;
            }


            this.data.itemdatas.Remove(item.data);

            if (Application.isPlaying)
                Destroy(item.gameObject);
#if UNITY_EDITOR
            else
                DestroyImmediate(item.gameObject);
#endif
        }

        /// <summary>
        /// 移除物体,通过格子索引删除，不指定引用
        /// </summary>
        public void RemoveItem(int index)
        {
            if (index >= 0 && index < this.slotList.Count)
            {
                var item = slotList[index].GetComponentInChildren<BaseItem<DATA, ENUM>>();
                if (item == null)
                {
                    DebugF.LogWarning($"slot[{index}] 处的没有东西可以移除");
                }

                if (item != null)
                    RemoveItem(item);
            }
            else
            {
                DebugF.LogWarning("索引超出范围");
            }
        }

        /// <summary>
        /// <para>1. 读取数据并 加载格子，物体到背包中</para>
        /// <para>2. iscopyData 是否拷贝一份新的DATA数据，默认为true。为false时DATA为引用数据</para>
        /// </summary>
        public void LoadALLDatas(InventoryData<DATA> bagData, bool iscopyData = true)
        {
            //重置背包
            ResetBag(bagData);

            //实例化DATA数据为Item对象
            foreach (var item_data in bagData.itemdatas)
            {
                AddItem<BaseItem<DATA, ENUM>>(item_data, item_data.slotIndex, iscopyData);
            }
        }

        /// <summary>
        /// 上传当前背包数据
        /// </summary>
        public InventoryData<DATA> ReturnALLDatas()
        {
            UpdateBag();
            return data;
        }

        /// <summary>
        /// 重置背包（删除物体，重载所有slot，但保留锁定信息）
        /// </summary>
        public void ResetBag(InventoryData<DATA> bagData)
        {
            UpdateSlotList();

            //清理所有格子
            foreach (var slot in slotList)
            {
                if (Application.isPlaying)
                {
                    Destroy(slot.gameObject);
                }
#if UNITY_EDITOR
                else
                {
                    DestroyImmediate(slot.gameObject);
                }
#endif
            }

            data.Clear();
            slotList.Clear();
            data.slot_count = bagData.slot_count; //更新格子数量
            data.locked_Index = bagData.locked_Index;//更新锁定信息

            //添加指定数量的格子
            for (int i = 0; i < bagData.slot_count; i++)
            {
                AddSlot();
            }


            //解锁所有格子
            foreach (var slot in slotList)
            {
                slot.IsLocked = false;
            }

            //锁定特定的格子
            foreach (var index in bagData.locked_Index)
            {
                if (index < slotList.Count)
                {
                    slotList[index].IsLocked = true;

                }
                else
                {
                    DebugF.LogWarning("索引超出现有格子数量");
                }
            }

        }

    }
}