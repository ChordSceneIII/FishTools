using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 动作管理器
/// 需要使用ActionManager的ScriptableObject实例注册BaseCommand实例
///
/// 采用优先级执行，BaseCommand配置OnExecute(),OnExit(),OnBreak()方法 来处理动作状态
///
/// BaseCommand之间的交互请使用事件订阅系统
///
/// </summary>

namespace FishTools.ActionSystem
{
    //动作处理器(单线程),权重处理器
    [CreateAssetMenu(fileName = "actionmanager", menuName = "FishTools/ActionSystem/Manager", order = 0)]
    public class ActionManager : ScriptableObject
    {
        private ActionManager()
        { }
        //序列化命令列表，编辑下引用
        [SerializeField]
        private List<BaseCommand> commandList = new List<BaseCommand>();

        //非序列化数据，请在使用ActionManager时初始化调用Initializer()
        private Dictionary<string, BaseCommand> commandPath = new Dictionary<string, BaseCommand>();

        //加载配置数据资源
        public static ActionManager LoadAsset(string pathInResources)
        {
            var manager = Resources.Load<ActionManager>(pathInResources);
            manager.Initializer();
            return manager;
        }

        //初始化容器
        internal void Initializer()
        {
            // 将命令按优先级排序
            commandList.Sort((cmd1, cmd2) => cmd2.Priority.CompareTo(cmd1.Priority));

            // 初始化命令字典
            commandPath.Clear();

            foreach (BaseCommand cmd in commandList)
            {
                commandPath.Add(cmd.command_name, cmd);
            }
        }


        //通过名称来执行命令(仅限于配置表中)
        public void ExecuteCommand(string command)
        {
            //使用Dictionary查找需要执行的命令
            if (commandPath.TryGetValue(command, out BaseCommand cmd))
            {
                Processor(cmd);
            }
            else
            {
                DebugF.LogWarning($"{this}: {command} is not found!");
            }
        }

        //通过命令来执行命令(可以是配置表外的，但是执行顺序是根据配置表的)
        public void ExecuteCommand(BaseCommand command)
        {
            Processor(command);
        }

        //查找命令
        public T FindCommand<T>(string command) where T : BaseCommand
        {
            if (commandPath.TryGetValue(command, out BaseCommand cmd))
            {
                if (cmd is T)
                    return cmd as T;
                else
                {
                    DebugF.LogWarning($"{this}: {command} is not {typeof(T)}!");
                    return null;
                }
            }
            else
            {
                DebugF.LogWarning($"{this}: {command} is not found!");
                return null;
            }
        }

        //执行命令之前检索权重器中的命令执行情况
        public void Processor(BaseCommand command)
        {
            if (command.IsActive)
            {
                foreach (BaseCommand cmd in commandList)
                {
                    //如果有权重更大的则阻止自己执行
                    if ((cmd.Priority > command.Priority) && cmd.IsExecuting)
                    {
                        return; // 有更高优先级的命令正在执行，不能执行当前命令
                    }
                    //打断正在执行的权重小的
                    else if ((command.Priority > cmd.Priority) && cmd.IsExecuting)
                    {
                        cmd.Break();
                    }
                }
                command.Execute();
            }
        }
    }
}