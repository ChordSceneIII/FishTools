using UnityEngine;
/// <summary>
/// 动作命令的基类，定义了基本的属性
/// </summary>
namespace FishTools.ActionSystem
{
    public abstract class BaseCommand : ScriptableObject
    {
        protected BaseCommand()
        { }

        public string command_name;

        [Tooltip("是否激活")]
        private bool isActive = true;
        public bool IsActive
        {
            get => isActive;
        }

        [SerializeField, Tooltip("是否正在执行")]
        private bool isExecuting = false;
        public bool IsExecuting
        {
            get => isExecuting;
        }

        [SerializeField, Tooltip("优先级")]
        private int priority = 0;
        public int Priority
        {
            get => priority;
        }

        public void Execute()
        {
            if (isActive)
            {
                isExecuting = true;
                DebugF.Log(GetType().Name + "执行中");
                OnExecute();
            }
            else
            {
                DebugF.Log(GetType().Name + "未激活");
            }
        }
        public void Exit()
        {
            if (isActive)
            {
                DebugF.Log(GetType().Name + "退出");
                isExecuting = false;
                OnExit();
            }
            else
            {
                DebugF.Log(GetType().Name + "未激活");
            }
        }

        public void Break()
        {
            if (isActive)
            {
                DebugF.Log(GetType().Name + "被打断");
                isExecuting = false;
                OnBreak();
            }
            else
            {
                DebugF.Log(GetType().Name + "未激活");
            }
        }

        public void SetActive(bool isactive = true)
        {
            isActive = isactive;
            if (isactive == false)
            {
                isExecuting = false;
            }
        }

        //执行方法
        protected abstract void OnExecute();

        //自然结束时的方法
        protected abstract void OnExit();

        //被打断时的方法
        protected abstract void OnBreak();

    }
}