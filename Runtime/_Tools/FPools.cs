using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishTools;

/// <summary>
/// 对象池
/// </summary>
namespace FishTools
{

    [Serializable]
    public class FPool
    {
        private FPool()
        { }
        private Queue<GameObject> pools_avail = new Queue<GameObject>();
        internal Queue<GameObject> PoolsAvail => pools_avail;
        [SerializeField] private List<GameObject> pools_used = new List<GameObject>();
        [Label("预制体")] public GameObject prefab;
        [Label("清理时间间隔")] public float clean_interval = 3f;
        [Label("每次销毁数量")] public float destory_every_count = 10;
        [Label("剩余时间"), SerializeField, ReadOnly] internal float remainingTime = 3f;
        #region 基本方法

        public bool Register(string key)
        {
            return FPoolManager.Instance.Register(key, this);
        }

        public bool UnRegister(string key)
        {
            return FPoolManager.Instance.UnRegister(key);
        }

        /// <summary>
        /// 获取对象（自定义初始化重置）
        /// </summary>
        /// <param name="init_action"></param>
        /// <returns></returns>
        public GameObject Get(Action<GameObject> init_action)
        {
            remainingTime = clean_interval;
            var obj = Get(null);
            init_action(obj);
            return obj;
        }

        /// <summary>
        /// 获取对象(Transform重置)
        /// </summary>
        public GameObject Get(Transform trans = null, Vector3 pos = default, Quaternion quaternion = default)
        {
            remainingTime = clean_interval;
            if (pools_avail.Count > 0)
            {
                var obj = pools_avail.Dequeue();
                pools_used.Add(obj);
                obj.transform.SetParent(trans);
                obj.transform.position = pos;
                obj.transform.rotation = quaternion;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                var obj = GameObject.Instantiate(prefab, pos, quaternion, trans);
                pools_used.Add(obj);
                return obj;
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool Remove(GameObject obj)
        {
            if (pools_used.Contains(obj))
            {
                obj.SetActive(false);
                pools_avail.Enqueue(obj);
                pools_used.Remove(obj);
                Create_Cleaner();
                return true;
            }
            return false;
        }

        public void Remove(GameObject obj, float delayTime)
        {
            FPoolManager.Instance.RemoveDelay(this, obj, delayTime);
        }

        /// <summary>
        /// 移除所有激活对象
        /// </summary>
        public void RemoveAll()
        {
            foreach (var obj in pools_used)
            {
                obj.SetActive(false);
                pools_avail.Enqueue(obj);
            }
            pools_used.Clear();
        }


        /// <summary>
        /// 清理对象池
        /// </summary>
        public void ClearPool()
        {
            foreach (var obj in pools_used)
            {
                GameObject.Destroy(obj);
            }
            foreach (var obj in pools_avail)
            {
                GameObject.Destroy(obj);
            }
            pools_used.Clear();
            pools_avail.Clear();
        }
        #endregion

        #region  预热机制
        public FPool PreWarm(int amount, Transform trans = null)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab, trans);
                obj.SetActive(false);
                pools_avail.Enqueue(obj);
            }
            return this;
        }
        #endregion

        #region  清理机制
        private Fmono cleaner = null;
        private void Create_Cleaner()
        {
            if (null == cleaner)
            {
                cleaner = FMonitor.Create($"清理器{this}")
                .Condition(() => pools_avail.Count == 0)
                .OnUpdate(() =>
                {
                    remainingTime -= Time.deltaTime;
                    if (remainingTime < 0)
                    {
                        //时机合适清理对象池多余物体内存空间
                        for (int i = 0; i < destory_every_count; i++)
                        {
                            if (PoolsAvail.Count > 0)
                                GameObject.Destroy(PoolsAvail.Dequeue());
                        }
                    }
                });
            }
        }

    }
}
#endregion

#region  对象池管理器
public class FPoolManager : BaseSingletonMono<FPoolManager>
{
    public Dictionary<string, FPool> allPools = new Dictionary<string, FPool>();
    Coroutine cleaner_coro;

    public FPool GetPool(string key)
    {
        return allPools.TryGetValue(key, out var pool) ? pool : null;
    }

    public bool Register(string key, FPool fPool)
    {
        if (allPools.ContainsKey(fPool.prefab.name))
        {
            DebugF.Log("对象池已存在");
            return false;
        }
        allPools.Add(key, fPool);
        return true;
    }

    public bool UnRegister(string name)
    {
        if (allPools.ContainsKey(name))
        {
            allPools.Remove(name);
            return true;
        }
        return false;
    }

    internal void RemoveDelay(FPool pool, GameObject obj, float delayTime)
    {
        StartCoroutine(RemoveDelay_Coro(pool, obj, delayTime));
    }

    internal IEnumerator RemoveDelay_Coro(FPool pool, GameObject obj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        pool.Remove(obj);
    }

}
#endregion

