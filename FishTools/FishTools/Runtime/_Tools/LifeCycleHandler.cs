using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FishTools
{
    /// <summary>
    /// 生命周期事件注册器
    /// </summary>
    public sealed class LifeCycleHandler : MonoBehaviour
    {
        // 可以选择的生命周期事件
        [System.Flags]
        public enum LifeCycleEvents
        {
            None = 0,
            Awake = 1 << 0,
            Start = 1 << 1,
            Update = 1 << 2,
            FixedUpdate = 1 << 3,
            LateUpdate = 1 << 4,
            OnDisable = 1 << 5,
            OnEnable = 1 << 6,
            Destroy = 1 << 7,
            ApplicationQuit = 1 << 8
        }

        public LifeCycleEvents activeEvents = LifeCycleEvents.None;

        [ConditionalField("activeEvents", LifeCycleEvents.Awake)] public UnityEvent onAwake;
        [ConditionalField("activeEvents", LifeCycleEvents.Start)] public UnityEvent onStart;
        [ConditionalField("activeEvents", LifeCycleEvents.Update)] public UnityEvent onUpdate;
        [ConditionalField("activeEvents", LifeCycleEvents.FixedUpdate)] public UnityEvent onFixedUpdate;
        [ConditionalField("activeEvents", LifeCycleEvents.LateUpdate)] public UnityEvent onLateUpdate;
        [ConditionalField("activeEvents", LifeCycleEvents.OnEnable)] public UnityEvent onEnable;

        [ConditionalField("activeEvents", LifeCycleEvents.OnDisable)] public UnityEvent onDisable;
        [ConditionalField("activeEvents", LifeCycleEvents.Destroy)] public UnityEvent onDestroy;
        [ConditionalField("activeEvents", LifeCycleEvents.ApplicationQuit)] public UnityEvent onApplicationQuit;

        private void Awake()
        {
            if ((activeEvents & LifeCycleEvents.Awake) != 0)
            {
                onAwake?.Invoke();
            }
            if (activeEvents.HasFlag(LifeCycleEvents.Awake))
            {
                onAwake?.Invoke();
            }
        }
        private void Start()
        {
            if ((activeEvents & LifeCycleEvents.Start) != 0)
            {
                onStart?.Invoke();
            }
        }
        private void Update()
        {
            if ((activeEvents & LifeCycleEvents.Update) != 0)
            {
                onUpdate?.Invoke();
            }
        }
        private void FixedUpdate()
        {
            if ((activeEvents & LifeCycleEvents.FixedUpdate) != 0)
            {
                onFixedUpdate?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if ((activeEvents & LifeCycleEvents.LateUpdate) != 0)
            {
                onLateUpdate?.Invoke();
            }
        }
        private void OnDisable()
        {
            if ((activeEvents & LifeCycleEvents.OnDisable) != 0)
            {
                onDisable?.Invoke();
            }
        }
        private void OnEnable()
        {
            if ((activeEvents & LifeCycleEvents.OnEnable) != 0)
            {
                onEnable?.Invoke();
            }
        }
        private void OnDestroy()
        {
            if ((activeEvents & LifeCycleEvents.Destroy) != 0)
            {
                onDestroy?.Invoke();
            }
        }
        private void OnApplicationQuit()
        {
            if ((activeEvents & LifeCycleEvents.ApplicationQuit) != 0)
            {
                onApplicationQuit?.Invoke();
            }
        }
    }
}
