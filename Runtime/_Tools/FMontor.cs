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
        /// </summary>
        public static Fmono Create(string name = "F_Monitor")
        {
            if (!Application.isPlaying) return null;
            var monitor = new GameObject($"{name}_{DateTime.Now}").AddComponent<Fmono>();
            monitor.StartCoroutine(monitor.Execute());
            return monitor;
        }

        public static Fmono CreateLoop(int loop_count, string name = "F_Monitor")
        {
            if (!Application.isPlaying) return null;
            var monitor = new GameObject($"{name}_{DateTime.Now}").AddComponent<Fmono>();
            monitor.StartCoroutine(monitor.Execute_Loop(loop_count));
            return monitor;
        }

        public static Fmono CreateForeach(int count, float interval = -1, string name = "F_Monitor")
        {
            if (!Application.isPlaying) return null;
            var monitor = new GameObject($"{name}_{DateTime.Now}").AddComponent<Fmono>();
            monitor.StartCoroutine(monitor.Execute_Foreach(count, interval));
            return monitor;
        }


        /// <summary>
        /// @物体完全销毁回调
        /// </summary>
        public static Fmono AfterDestory<T>(T obj) where T : Component
        {
            if (!Application.isPlaying) return null;

            if (!FishUtility.IsNull(obj)) GameObject.Destroy(obj.gameObject);

            return Create("销毁监控").Condition(() => obj.gameObject == null);
        }

        /// <summary>
        /// @所有物体完全销毁回调
        /// </summary>
        public static Fmono AfterDestory<T>(List<T> objs) where T : Component
        {
            if (!Application.isPlaying) return null;

            return Create($"销毁监控:[{objs.Count}]")
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
    }
    #region 工具Mono脚本

    // 观察者脚本，检测物体是否已满足条件
    public sealed class Fmono : MonoBehaviour
    {
        private event Action onUpdateEvent;
        private event Action onCompleteEvent;
        private event Action onStartEvent;
        private float delayTime = 0;
        private float extendTime = 0;
        private Func<bool> condition = null;
        private bool includeExtendTime = false;
        private bool realTime = false;

        public Fmono OnUpdate(Action action)
        {
            onUpdateEvent += action; // 订阅事件
            return this;
        }
        public Fmono OnComplete(Action action)
        {
            onCompleteEvent += action; // 订阅事件
            return this;
        }
        public Fmono OnStart(Action action)
        {
            onStartEvent += action; // 订阅事件
            return this;
        }

        /// <summary>
        /// 延迟时间
        /// </summary>
        public Fmono Delay(float duration)
        {
            delayTime = duration;
            return this;
        }

        /// <summary>
        /// 延后时间(只影响结束时间)
        /// </summary>
        public Fmono Extend(float duration, bool addToUpdate = false)
        {
            extendTime = duration;
            this.includeExtendTime = addToUpdate;
            return this;
        }

        /// <summary>
        /// 是否使用真实时间
        /// </summary>
        public Fmono RealTime(bool realtime)
        {
            this.realTime = realtime;
            return this;
        }

        /// <summary>
        /// 结束条件
        /// </summary>
        public Fmono Condition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }

        /// <summary>
        /// 执行
        /// </summary>
        internal IEnumerator Execute()
        {
            yield return Execute_Coro();
            Destroy(gameObject);
        }

        /// <summary>
        /// 执行(循环)
        /// </summary>
        internal IEnumerator Execute_Loop(int loop_count = 0)
        {
            for (int i = 0; i < loop_count; i++)
            {
                yield return Execute_Coro();
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// 执行(集中)
        /// </summary>
        internal IEnumerator Execute_Foreach(int count, float interval = -1)
        {
            for (int i = 0; i < count - 1; i++)
            {
                StartCoroutine(Execute_Coro());
                if (interval >= 0)
                {
                    yield return new WaitForSeconds(interval);
                }
            }
            yield return Execute_Coro();
            Destroy(gameObject);
        }

        private IEnumerator Execute_Coro()
        {
            yield return null;

            if (realTime)
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
                float startTime = realTime ? Time.realtimeSinceStartup : Time.time;
                while (true)
                {
                    if (includeExtendTime)
                    {
                        onUpdateEvent?.Invoke();
                    }

                    float elapsed = realTime
                        ? Time.realtimeSinceStartup - startTime
                        : Time.time - startTime;

                    if (elapsed >= extendTime)
                        break;

                    yield return null;
                }
            }

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