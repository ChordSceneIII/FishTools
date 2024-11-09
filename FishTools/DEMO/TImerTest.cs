using UnityEngine;
using UnityEngine.UI;
using FishTools.TimeTool;
using System;


namespace FishToolsDEMO
{
    public class TImerTest : MonoBehaviour
    {
        public Text text;
        public Button StartBtn;
        public Button StopBtn;
        public Button ContinueBtn;
        public Button ResetBtn;
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

            StartBtn.onClick.AddListener(() =>
            {
                timer.Start();
                timer.SetLoop(loopTimes)
                .OnInterval(intervalTime, cure, () =>
                 {
                     Debug.Log("间隔回调");
                 }); ;
            });

            StopBtn.onClick.AddListener(() =>
            {
                timer.Stop();
            });

            ContinueBtn.onClick.AddListener(() =>
            {
                timer.Continue();
            });

            ResetBtn.onClick.AddListener(() =>
            {
                timer.Close();
            });

            timer.OnCompleteUpdate(() =>
            {
                Debug.Log("结束持续触发");
            });
            timer.OnContinue(() =>
            {
                Debug.Log("恢复暂停回调");

            });
            timer.OnUpdate(() =>
            {
                // Debug.Log("持续回调");
            });

            timer.OnStart(() =>
            {
                Debug.Log("开始回调");
            });
            timer.OnComplete(() =>
            {
                Debug.Log("完成回调");
            });

            timer.OnStop(() =>
            {
                Debug.Log("暂停回调");
            });
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