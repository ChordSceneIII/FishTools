using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishTools.InputTool;
using FishTools;

namespace FishToolsDEMO
{

    public class TestListener : MonoBehaviour
    {
        KeyMapper key;
        KeyMapper key2;

        KeyMapper key3;

        public float durationLongPress;

        private void Start()
        {
            key = KeyMapper.InputManager.ComplexKeyUp("key1", KeyCode.A, KeyCode.D);
            InputManager.Instance.AddEventsByMapperName("key1", MYPrint);

            key2 = InputManager.Instance.GetKey("key2", KeyCode .G);

            var keyf = InputManager.Instance.FindMapper<GetKey>("key2");
            keyf.AddEvent(MYPrint2);

            key3 = KeyMapper.InputManager.ShortPress("longpress space", KeyCode.Tab, durationLongPress);
            key3.AddEvent(() => { Debug.Log("长按测试"); });
        }
        private void OnEnable()
        {
            EventManager.Instance.AddEventListener<int>("SunNumChange", CheckUnlock);
        }

        void MYPrint()
        {
            Debug.Log($"输入映射测试");
        }
        void MYPrint2()
        {
            Debug.Log($"输入映射测ssssss试");
        }

        void CheckUnlock(int num)
        {
            Debug.Log($"增加{num}");
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener<int>("SunNumChange", CheckUnlock);
            key?.RemoveEvent(MYPrint);
        }
    }
}