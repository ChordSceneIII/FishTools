using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 销毁相关的封装。用于销毁物体时插入回调至OnDestory 以及 Destroy之后的回调
/// </summary>

namespace FishTools
{
    public static class GameObjectUtility
    {
        //维持层级位置的替换
        public static void ReplaceTransform(GameObject oldObject, GameObject newObject)
        {
            if (oldObject == null || newObject == null)
            {
                Debug.LogWarning("旧对象或新 prefab 未设置。");
                return;
            }

            // 将新对象设为旧对象的父级
            oldObject.transform.SetParent(newObject.transform);

            // 销毁旧对象
            GameObject.Destroy(oldObject);
        }

        public static void OnDestroy(GameObject obj, Action onDestroyed)
        {
            if (obj == null)
            {
                DebugF.LogWarning("Cannot destroy a null object.");
                return;
            }

            // 创建一个用于回调的临时脚本
            var callbackHandler = obj.AddComponent<DestroyCallbackHandler>();
            callbackHandler.onDestroyed = onDestroyed;
            // 销毁对象
            UnityEngine.Object.Destroy(obj);
        }

        public static void AfterDestroy(GameObject obj, Action onDestroyed)
        {
            if (obj == null)
            {
                Debug.LogWarning("不能销毁一个空物体");
                return;
            }

            UnityEngine.Object.Destroy(obj);  // 立即销毁对象

            // 启动等待销毁的检查协程
            var destroyWatcher = new GameObject("DestroyObserver").AddComponent<DestroyObserver>();
            destroyWatcher.StartCoroutine(destroyWatcher.WaitForDestroy(obj, onDestroyed));
        }
    }

    // 临时脚本，用于处理 OnDestroy 回调
    public class DestroyCallbackHandler : MonoBehaviour
    {
        public Action onDestroyed;

        private void OnDestroy()
        {
            onDestroyed?.Invoke();  // 在对象销毁后执行回调
        }
    }

    //Destory观察者脚本，检测物体已经正确被销毁
    public class DestroyObserver : MonoBehaviour
    {
        public IEnumerator WaitForDestroy(GameObject obj, Action onDestroyed)
        {
            // 等待直到对象完全销毁
            while (obj != null)
            {
                yield return null;
            }

            // 完全销毁后执行回调
            onDestroyed?.Invoke();

            // 销毁 DestroyWatcher 自身
            Destroy(gameObject);
        }
    }
}