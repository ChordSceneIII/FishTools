using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 消息订阅系统(观察者模式)
///
/// 使用EventTrigger()来注册触发事件
/// 使用AddEventListener()来监听事件并设置相应方法
/// </summary>

namespace FishTools
{
    public static class EventManager
    {
        //存储事件列表
        private static Dictionary<string, IEventInfo> _eventDic = new Dictionary<string, IEventInfo>();

        /// <summary>
        /// 注册事件
        /// </summary>
        public static void AddListener(string name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions += action;
            }
            else
            {
                _eventDic.Add(name, new EventInfo(action));
            }
        }

        /// <summary>
        /// 注册带参事件
        /// </summary>
        public static void AddListener<T>(string name, UnityAction<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>).actions += action;
            }
            else
            {
                _eventDic.Add(name, new EventInfo<T>(action));
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        public static void Trigger(string name)
        {
            if (_eventDic.ContainsKey(name))
            {
                if ((_eventDic[name] as EventInfo).actions != null)
                {
                    (_eventDic[name] as EventInfo).actions.Invoke();
                }
            }
        }


        /// <summary>
        /// 触发带参事件
        /// </summary>
        public static void Trigger<T>(string name, T parameter)
        {
            if (_eventDic.ContainsKey(name))
            {
                if ((_eventDic[name] as EventInfo<T>).actions != null)
                {
                    (_eventDic[name] as EventInfo<T>).actions.Invoke(parameter);
                }
            }
        }
        public static void RemoveListener(string name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions -= action;
            }
        }
        public static void RemoveListener<T>(string name, UnityAction<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>).actions -= action;
            }
        }

        /// <summary>
        /// 清空事件列表
        /// </summary>
        public static void Clear()
        {
            _eventDic.Clear();
        }

        /// <summary>
        /// 打印所有事件名称
        /// </summary>
        public static void PrintKey()
        {
            foreach (var item in _eventDic)
            {
                DebugF.Log(item.Key);
            }
        }
    }
    //TODO:思考一下是否能够同时注册相同name的带参和不带参事件

    public interface IEventInfo
    {
    }

    public class EventInfo : IEventInfo
    {
        public UnityAction actions; //使用UnityAction
        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }

    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }

    }
}
