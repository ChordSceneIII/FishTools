using UnityEngine;
using UnityEngine.UI;
using FishTools.TimeTool;
using System;


namespace FishToolsDEMO
{
    public class TImerTestDisplay : MonoBehaviour
    {
        public Text text;
        public float duration;
        public int loopTimes;
        public int intervalTime;
        private FTimer timer;
        private FTimer timer2;
        public AnimationCurve cure;
        void OnEnable()
        {
            timer = FTM.CreateTimer(duration);
            timer2 = FTM.CreateTimer(duration);
        }

        public void StartTimer()
        {

            timer.Start()
            .SetLoop(loopTimes)
            .OnInterval(intervalTime, cure, () =>
             {
                 Debug.Log("间隔回调");
             })
            .OnContinue(() =>
            {
                Debug.Log("恢复暂停回调");
            })
            .OnUpdate(() =>
            {
                // Debug.Log("持续回调");
            })
            .OnStart(() =>
            {
                Debug.Log("开始回调");
            })
            .OnComplete(() =>
            {
                Debug.Log("完成回调");
            })
            .OnCompleteUpdate(() =>
            {
                // Debug.Log("结束持续触发");
            })
            .OnStop(() =>
            {
                Debug.Log("暂停回调");
            });

        }
        public void StopTimer()
        {
            timer.Stop();
        }
        public void ContinueTimer()
        {
            timer.Continue();
        }
        public void ResetTimer()
        {
            timer.Close();
        }

        private void Update()
        {
            string forwardTime = timer.GetForwardTime().ToString();
            string resultTime = timer.GetResultTime().ToString();
            text.text = $"正向时间: {forwardTime} + 反向时间: {resultTime}";
        }

        void OnDisable()
        {
            FTM.RemoveTimer(timer);
            FTM.RemoveTimer(timer2);
        }
    }

}