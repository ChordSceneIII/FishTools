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
    public class EventManager : BaseManager<EventManager>
    {
        //存储事件列表
        private Dictionary<string, IEventInfo> _eventDic = new Dictionary<string, IEventInfo>();

        public void AddEventListener(string name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions += action;
            }
            else
            {
                //添加事件
                _eventDic.Add(name, new EventInfo(action));
            }
        }

        public void AddEventListener<T>(string name, UnityAction<T> action)
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

        public void EventTrigger(string name)
        {
            if (_eventDic.ContainsKey(name))
            {
                if ((_eventDic[name] as EventInfo).actions != null)
                {
                    (_eventDic[name] as EventInfo).actions.Invoke();
                }
            }
        }

        public void EventTrigger<T>(string name, T parameter)
        {
            if (_eventDic.ContainsKey(name))
            {
                if ((_eventDic[name] as EventInfo<T>).actions != null)
                {
                    (_eventDic[name] as EventInfo<T>).actions.Invoke(parameter);
                }
            }
        }
        public void RemoveEventListener(string name, UnityAction action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo).actions -= action;
            }
        }
        public void RemoveEventListener<T>(string name, UnityAction<T> action)
        {
            if (_eventDic.ContainsKey(name))
            {
                (_eventDic[name] as EventInfo<T>).actions -= action;
            }
        }

        //在需要时清理列表
        public void Clear()
        {
            _eventDic.Clear();
        }
    }

    public interface IEventInfo
    {
    }

    public class EventInfo : IEventInfo
    {
        public UnityAction actions;
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
