using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishTools
{
    public static class FMonitor
    {
        /// <summary>
        /// <para>@条件监视器</para>
        /// <para>@: uscale=true代表不受TimeScale影响</para>
        /// </summary>
        public static F_Monitor Create(bool unscale = false, string name = "F_Monitor")
        {
            if (!Application.isPlaying) return null;
            // 创建一个观察者对象
            var monitor = new GameObject($"{name}_{DateTime.Now}").AddComponent<F_Monitor>();
            monitor.StartCoroutine(monitor.Execute(unscale));
            return monitor;
        }

        /// <summary>
        /// <para>@条件监视器</para>
        /// <para>@: uscale=true代表不受TimeScale影响</para>
        /// </summary>
        public static F_Monitor Create(string name = "F_Monitor", bool unscale = false)
        {
            return Create(unscale, name);
        }

        public static F_Monitor Create()
        {
            return Create(false, "F_Monitor");
        }

        /// <summary>
        /// @物体完全销毁回调
        /// </summary>
        public static F_Monitor AfterDestory<T>(T obj) where T : Component
        {
            if (!Application.isPlaying) return null;

            if (!FishUtility.IsNull(obj)) GameObject.Destroy(obj.gameObject);

            return Create(true, "销毁监控").Condition(() => obj.gameObject == null);
        }

        /// <summary>
        /// @所有物体完全销毁回调
        /// </summary>
        public static F_Monitor AfterDestory<T>(List<T> objs) where T : Component
        {
            if (!Application.isPlaying) return null;

            return Create(true, $"销毁监控:[{objs.Count}]")
            .OnStart(() =>
            {
                foreach (var obj in objs)
                {
                    if (!FishUtility.IsNull(obj)) GameObject.Destroy(obj.gameObject);
                }
            })
            .Condition(() =>
            {
                foreach (var obj in objs)
                {
                    if (!FishUtility.IsNull(obj))
                        return false;
                }
                return true;
            });
        }


        #region 工具Mono脚本

        // 观察者脚本，检测物体是否已满足条件
        public sealed class F_Monitor : MonoBehaviour
        {
            private event Action onUpdateEvent;
            private event Action onCompleteEvent;
            private event Action onStartEvent;
            private float delayTime = 0;
            private float extendTime = 0;
            private Func<bool> condition = null;
            private bool includeExtendTime = false;

            public F_Monitor OnUpdate(Action action)
            {
                onUpdateEvent += action; // 订阅事件
                return this;
            }
            public F_Monitor OnComplete(Action action)
            {
                onCompleteEvent += action; // 订阅事件
                return this;
            }
            public F_Monitor OnStart(Action action)
            {
                onStartEvent += action; // 订阅事件
                return this;
            }

            /// <summary>
            /// 延迟时间
            /// </summary>
            public F_Monitor Delay(float duration)
            {
                delayTime = duration;
                return this;
            }

            /// <summary>
            /// 延后时间
            /// </summary>
            public F_Monitor Extend(float duration, bool addToUpdate = false)
            {
                extendTime = duration;
                this.includeExtendTime = addToUpdate;
                return this;
            }

            /// <summary>
            /// 结束条件
            /// </summary>
            public F_Monitor Condition(Func<bool> condition)
            {
                this.condition = condition;
                return this;
            }

            internal IEnumerator Execute(bool unscale = false)
            {
                yield return null;

                if (unscale)
                    yield return new WaitForSecondsRealtime(delayTime);
                else
                    yield return new WaitForSeconds(delayTime);

                onStartEvent?.Invoke();

                yield return new WaitForEndOfFrame();

                // 条件不满足时一直执行
                while (condition != null && !condition())
                {
                    onUpdateEvent?.Invoke();
                    yield return null;
                }

                if (extendTime > 0)
                {
                    float startTime = unscale ? Time.realtimeSinceStartup : Time.time;
                    while (true)
                    {
                        if (includeExtendTime)
                        {
                            onUpdateEvent?.Invoke();
                        }

                        float elapsed = unscale
                            ? Time.realtimeSinceStartup - startTime
                            : Time.time - startTime;

                        if (elapsed >= extendTime)
                            break;

                        yield return null;
                    }
                }

                // 销毁监视器对象自身
                Destroy(gameObject);

                // 条件满足后执行回调
                onCompleteEvent?.Invoke();
            }

            private void OnDestroy()
            {
                // 清空事件订阅,防止悬挂
                onUpdateEvent = null;
                onCompleteEvent = null;
                onStartEvent = null;
                condition = null;
            }
        }
        #endregion
    }
}