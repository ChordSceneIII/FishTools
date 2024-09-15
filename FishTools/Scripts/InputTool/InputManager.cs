using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FishTools;


/// <summary>
/// 基于旧InputManager封装的Input工具
/// 使用消息订阅的设计模式
/// 输入检测默认就是在Update中，没什么好说的，Fixed处理物理操作请另外做
/// </summary>

namespace InputTool
{
    public partial class InputManager : BaseSingletonMono<InputManager>
    {
        #region MonoLife
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        //静态列表，脱离Mono生命周期方便再编辑器中管理
        internal static List<KeyMapper> mapperList = new List<KeyMapper>();

        //字典，用于映射配置的管理
        internal static Dictionary<string, KeyMapper> mapperDic = new Dictionary<string, KeyMapper>();

        void Update()
        {
            foreach (var mapper in mapperList)
            {
                if (mapper.Check())
                {
                    mapper.Execute(false);
                }
                else
                {
                    mapper.Execute(true);
                }
            }
        }
        #endregion

        #region Mapper表管理
        //添加配置
        public bool AddMapper(string name, KeyMapper mapper)
        {
            if (!mapperDic.ContainsKey(name))
            {
                mapperList.Add(mapper);
                mapperDic.Add(name, mapper);
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"{name}配置已经存在");
#endif
                return false;
            }
        }

        //移除配置
        public bool RemoveMapper(string name)
        {
            if (mapperDic.TryGetValue(name, out var mapper))
            {
                mapperList.Remove(mapper);
                mapperDic.Remove(name);
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"{name}配置不存在");
#endif
                return false;
            }
        }

        //查找配置
        public T FindMapper<T>(string mappername) where T : KeyMapper
        {
            if (mapperDic.TryGetValue(mappername, out var mapper) && mapper is T)
            {
                return mapper as T;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"{mappername}配置不存在");
#endif
                return null;
            }
        }

        //通过映射名称添加事件
        public void AddEventsByMapperName(string mappername, UnityAction action)
        {
            if (mapperDic.TryGetValue(mappername, out var mapper))
            {
                mapper.AddEvent(action);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"{mappername}配置不存在");
#endif
            }
        }

        //清空所有配置
        public void ClearMapple()
        {
            mapperDic.Clear();
            mapperList.Clear();
#if UNITY_EDITOR
            Debug.Log("清空所有配置");
#endif
        }
        #endregion
    }
}