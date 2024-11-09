using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// KeyMapper基类
/// </summary>

namespace FishTools.InputTool
{
    public abstract class KeyMapper : IDisposable
    {
        protected UnityEvent actions = new UnityEvent();
        protected UnityEvent actions_opposite = new UnityEvent();

        protected string mappername;

        //执行逻辑
        public abstract bool Check();

        //保存按键配置
        public abstract void SaveKey();

        //加载按键配置
        protected abstract void LoadKey();

        // 可选的通用实现 使用PlayerPrefs读取和保存键位偏好
        protected void SaveKey(string key, KeyCode keyCode)
        {
            PlayerPrefs.SetInt($"{mappername}_{key}", (int)keyCode);
            PlayerPrefs.Save();
        }

        protected void LoadKey(string key, ref KeyCode keyCode)
        {
            if (PlayerPrefs.HasKey($"{mappername}_{key}"))
            {
                keyCode = (KeyCode)PlayerPrefs.GetInt($"{mappername}_{key}");
            }
        }

        //执行动作
        internal virtual void Execute(bool isOpposite)
        {
            if (isOpposite == false)
                actions?.Invoke();
            else if (isOpposite == true)
                actions_opposite?.Invoke();
        }
        //添加事件
        public void AddEvent(UnityAction action, bool isOpposite = false)
        {
            if (isOpposite == false)
                this.actions.AddListener(action);
            if (isOpposite == true)
                this.actions_opposite.AddListener(action);
        }

        //移除事件
        public void RemoveEvent(UnityAction action, bool isOpposite = false)
        {
            if (isOpposite == false)
                this.actions.RemoveListener(action);
            if (isOpposite == true)
                this.actions_opposite.RemoveListener(action);
        }

        //清理所有事件
        public void Dispose()
        {
            // 清除 actions 的所有监听器
            actions.RemoveAllListeners();
            actions_opposite.RemoveAllListeners();
        }

        protected KeyMapper() { }

        public static InputManager InputManager => InputManager.Instance;
    }

}
