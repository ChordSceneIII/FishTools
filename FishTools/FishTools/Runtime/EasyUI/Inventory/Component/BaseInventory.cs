using UnityEngine;
using System.Collections.Generic;
using System;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 背包基类拓展
    /// </summary>
    public abstract class BaseInventory<DATA, TYPE, PATH> : MonoBehaviour, IBaseInventory where DATA : BaseItemDATA<TYPE> where TYPE : Enum where PATH : BaseItemPath<TYPE>
    {

        #region #### 属性设置
        [Label("路径配置")] public PATH pathConfig;
        [Label("Slot预制体")] public GameObject slotPrefab;
        [Label("Slot前缀名")] public string sample_name = "slot_";
        [Label("Slot选择事件")] public string slotSelectKey;
        [Label("Slot确认事件")] public string slotSubmitKey;
        private List<Slot> slotList = new List<Slot>();
        public List<Slot> Slots => slotList;
        private List<BaseItem<DATA, TYPE>> itemList = new List<BaseItem<DATA, TYPE>>();
        public List<BaseItem<DATA, TYPE>> Items => itemList;
        public List<DATA> itemdatas = new List<DATA>();
        [Label("缓存数据"), SerializeField] public InventoryData<DATA> _data = new InventoryData<DATA>();
        #endregion

        /// <summary>
        /// 添加一个格子
        /// </summary>
        public Slot AddSlot(string name, bool isLocked)
        {
            if (slotPrefab == null || slotPrefab.GetComponent<Slot>() == null)
            {
                DebugF.LogError("Slot预制体为空或者预制体上没有Slot组件");
                return null;
            }

            GameObject new_slot = Instantiate(slotPrefab, transform);
            Slot slot = new_slot.GetComponent<Slot>();
            Slots.Add(slot);
            slot.selectKey = slotSelectKey;
            slot.submitKey = slotSubmitKey;
            slot.IsLocked = isLocked;
            new_slot.name = name;
            return slot;
        }

        /// <summary>
        /// 添加一个格子（默认方式）
        /// </summary>
        public Slot AddSlot(bool isLocked = false)
        {
            return AddSlot(sample_name + Slots.Count, isLocked);
        }

        /// <summary>
        /// 从末尾队列移除一个格子
        /// </summary>
        public void RemoveSlot()
        {
            if (Slots.Count > 0)
            {
                Slot last_slot = slotList[Slots.Count - 1];

                if (Application.isPlaying)
                {
                    Slots.Remove(last_slot);
                    Destroy(last_slot.gameObject);
                }
                else
                {
                    Slots.Remove(last_slot);
                    DestroyImmediate(last_slot.gameObject);
                }
            }
        }

        /// <summary>
        /// 更新列表结构
        /// </summary>
        public void UpdateList()
        {
            Slots.Clear();
            Items.Clear();
            itemdatas.Clear();
            foreach (Transform child in transform)
            {
                var slot = child.GetComponentInChildren<Slot>();
                if (slot != null) Slots.Add(slot);

                var item = slot?.GetComponentInChildren<BaseItem<DATA, TYPE>>();
                if (item != null)
                {
                    Items.Add(item);
                    itemdatas.Add(item.data);
                }
            }
        }

        /// <summary>
        /// 寻找未被锁定的空背包格
        /// </summary>
        public int FindEmptySlot(bool includeLocked = false)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot slot = Slots[i];
                if (slot.IItem == null && (!slot.IsLocked || includeLocked))
                {
                    // DebugF.Log($"找到空格子，序号为：{i}");
                    return i;
                }
            }

            DebugF.Log("未找到可用格子");
            return -1;
        }

        /// <summary>
        /// 判断背包是否已满
        /// </summary>
        public bool IsFull()
        {
            if (Slots.Count == Items.Count)
            {
                return true;
            }

            DebugF.Log("背包已满");
            return true;
        }

        /// <summary>
        /// 寻找指定类型的物体
        /// </summary>
        public T FindItemByType<T>(TYPE type) where T : BaseItem<DATA, TYPE>
        {
            foreach (var item in Items)
            {
                if (item != null && item.Type.Equals(type))
                {
                    return item as T;
                }
            }
            return null;
        }

        /// <summary>
        /// 添加物体(指定slot索引)
        /// </summary>
        public T AddItem<T>(DATA data, int slotIndex, bool iscopy = true) where T : BaseItem<DATA, TYPE>
        {
            if (pathConfig == null)
            {
                DebugF.LogError("没有配置路径");
                return null;
            }

            if (data == null)
            {
                DebugF.LogError("不能导入空数据");
                return null;
            }
            if (!pathConfig.TryGetPrefab(data.type, out var prefab))
            {
                DebugF.LogError("预制体不存在或者未正确配置,请检查pathConfig");
                return null;
            }

            if (slotIndex < 0 || slotIndex >= slotList.Count)
            {
                DebugF.Log("索引超出现有格子范围");
                return null;
            }

            var slot = Slots[slotIndex];
            var obj = Instantiate(prefab, slot.transform);
            var new_item = obj.GetComponent<T>();

            if (iscopy)
            {
                new_item.data = FishUtility.DeepCopy(data);//拷贝数据到新的对象上
            }
            else
            {
                new_item.data = data;//引用数据到新的对象上
            }

            Items.Add(new_item);
            itemdatas.Add(new_item.data);
            new_item.AddCount(0);//更新数量变化
            return new_item;
        }

        /// <summary>
        /// 添加物体(自动寻找空位)
        /// </summary>
        public T AddItem<T>(DATA data, bool iscopy = true, bool includeLocked = false) where T : BaseItem<DATA, TYPE>
        {
            var index = FindEmptySlot(includeLocked);

            if (index < 0)
            {
                DebugF.Log("没有空格子");
                return null;
            }
            return AddItem<T>(data, index, iscopy);
        }

        /// <summary>
        /// 通过引用移除
        /// </summary>
        public void RemoveItem<T>(T item) where T : BaseItem<DATA, TYPE>
        {
            if (item == null)
            {
                DebugF.LogWarning("无法移除空物体");
                return;
            }

            if (Application.isPlaying)
            {
                Items.Remove(item);
                itemdatas.Remove(item.data);

                Destroy(item.gameObject);
            }
            else
            {
                Items.Remove(item);
                itemdatas.Remove(item.data);
                DestroyImmediate(item.gameObject);
            }
        }

        /// <summary>
        /// 移除物体,通过格子索引删除
        /// </summary>
        public void RemoveItem(int index)
        {
            if (index >= 0 && index < this.Slots.Count)
            {
                var iitem = Slots[index].IItem;
                if (FishUtility.IsNull(iitem))
                {
                    DebugF.LogWarning($"slot[{index}] 处的没有东西可以移除");
                    return;
                }
                else
                {
                    var item = iitem.gameObject.GetComponentInChildren<BaseItem<DATA, TYPE>>();
                    RemoveItem(item);
                }
            }
            else
            {
                DebugF.LogWarning("索引超出范围");
            }
        }

        /// <summary>
        /// 获取当前数据(一般都在需要存档时调用)
        /// </summary>
        public InventoryData<DATA> GetData()
        {
            UpdateList();

            //清空缓存
            var cache = new InventoryData<DATA>();

            //更新缓存数据
            cache.slot_count = Slots.Count;

            //更新锁定格子的索引
            foreach (var slot in Slots)
            {
                if (slot != null && slot.IsLocked)
                {
                    cache.locked_Index.Add(Slots.IndexOf(slot));
                }
            }

            //更新物品数据
            foreach (var item in Items)
            {
                if (item != null)
                {
                    //更新item对应的slot索引
                    item.data.slotindex = Slots.IndexOf(item.OfSlot);
                    //深拷贝数据
                    DATA itemdata = FishUtility.DeepCopy(item.data);
                    //上传至缓存
                    cache.items.Add(itemdata);
                }
            }

            return cache;
        }

        private const string color_success = "#D0B080";
        /// <summary>
        /// 缓存数据
        /// </summary>
        public void Cache()
        {
            _data = GetData();
            DebugF.LogColor(color_success, "数据缓存成功");
        }

        /// <summary>
        /// <para>1. 下载数据</para>
        /// <para>2. iscopyData 为是否重新拷贝一份数据</para>
        /// </summary>
        public void Download(InventoryData<DATA> data, bool IsDeepCopy = true)
        {
            //清空背包（包括格子和物品）
            Clear();

            //实例化格子
            for (int i = 0; i < data.slot_count; i++)
            {
                bool isLocked = data.locked_Index.Contains(i);
                AddSlot(isLocked);
            }

            //实例化Item，并填充数据
            foreach (var item_data in data.items)
            {
                AddItem<BaseItem<DATA, TYPE>>(item_data, item_data.slotindex, IsDeepCopy);
            }
            DebugF.LogColor(color_success, "数据下载成功(Include Slot)");
        }

        /// <summary>
        /// 只记录物体不管格子(分类，预览，重装时可能会用到)
        /// </summary>
        public void Download_OnlyItem(InventoryData<DATA> data, bool IsDeepCopy = true)
        {
            Clear();

            //实例化格子
            for (int i = 0; i < data.items.Count; i++)
            {
                bool isLocked = data.locked_Index.Contains(i);
                AddSlot($"{sample_name}{i}_only", isLocked);
            }

            //实例化Item，并填充数据
            foreach (var item_data in data.items)
            {
                AddItem<BaseItem<DATA, TYPE>>(item_data, IsDeepCopy);
            }
            // DebugF.LogColor(color_success, "数据下载成功(Only Item)");
        }

        /// <summary>
        /// 重新加载背包
        /// </summary>
        public void ReloadCache()
        {
            Download(_data);
        }

        /// <summary>
        /// 更新缓存并上传
        /// </summary>
        public InventoryData<DATA> Upload()
        {
            //更新缓存
            Cache();
            DebugF.LogColor(color_success, "数据上传成功");
            return _data;
        }

        /// <summary>
        /// 清空背包(实例和数据)
        /// </summary>
        public void Clear()
        {
            UpdateList();

            var slotsToDestroy = new List<Slot>(Slots);

            foreach (var slot in slotsToDestroy)
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
            Slots.Clear();
            Items.Clear();
            itemdatas.Clear();
        }

        public void ClearItem()
        {
            UpdateList();

            var itemToDestroy = new List<BaseItem<DATA, TYPE>>(Items);

            foreach (var item in itemToDestroy)
            {
                if (Application.isPlaying)
                {
                    Destroy(item.gameObject);
                }
#if UNITY_EDITOR
                else
                {
                    DestroyImmediate(item.gameObject);
                }
#endif
            }
            Items.Clear();
            itemdatas.Clear();
        }

    }
}