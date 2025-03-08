using UnityEngine;
using System.Collections.Concurrent;
using System;
using System.Threading;

/// <summary>
/// 多线程工具:用于I/O，磁盘访问，资源下载等费时的处理
/// </summary>

namespace FishTools
{
    // // 示例
    // public class TestLoom : MonoBehaviour
    // {
    //     void Start()
    //     {
    //         // 创建一个新线程
    //         Loom.RunAsync(() =>
    //         {
    //             Debug.Log("Running in background thread");
    //             Thread.Sleep(2000); // 模拟长时间运行的任务

    //             // 回到主线程
    //             Loom.QueueOnMainThread(() =>
    //             {
    //                 Debug.Log("Back to main thread");
    //             });
    //         });
    //     }
    // }

    public class Loom : MonoBehaviour
    {
        public static int maxThreads = 8;
        static int numThreads;
        private static Loom _instance;
        public static Loom Instance
        {
            get
            {
                Initialize();
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
            initialized = true;
        }

        static bool initialized;

        static void Initialize()
        {
            if (!initialized)
            {
                if (!Application.isPlaying)
                    return;
                initialized = true;
                var g = new GameObject("Loom");
                _instance = g.AddComponent<Loom>();
                DontDestroyOnLoad(g);
            }
        }

        private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        public static void QueueOnMainThread(Action action)
        {
            Instance._actions.Enqueue(action);
        }

        public static Thread RunAsync(Action a)
        {
            Initialize();
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
                // handle exception
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }

        void OnDisable()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        void Update()
        {
            while (_actions.TryDequeue(out var action))
            {
                action();
            }
        }
    }
}
