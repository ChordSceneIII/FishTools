using System;
using UnityEngine;

namespace TimerTool
{
    [Serializable]
    public class FTimer : IDisposable
    {

#if UNITY_EDITOR

        public float DisplayTime => GetResultTime();
        public bool IsCompleted => isCompleted;
        public int ResultLoop => LoopMAX - LoopCUR;
        public AnimationCurve Curve => curve;

#endif

        private bool isRunning;//是否正在运行，由Stop（）和Continue（）控制，同时控制事件
        private bool isCompleted;//是否完成，仅仅作为标志
        private bool isActive;//是否激活，由Start（）和Close（）控制
        private float duration; //持续时间
        private float startPoint;//开始的时间点
        private float stopPoint;//停止时剩余时间
        private float endPoint;//结束的时间点
        private int LoopMAX = 1;//循环次数
        private int LoopCUR = 1;//当前已循环次数
        private AnimationCurve curve; //间隔方法的曲线
        private int intervalNum = 0; // 间隔数
        private int currentIntervalIndex = 0; // 当前间隔事件的索引

        private Action onUpdate;
        private Action onCompleteUpdate;
        private Action onStart;
        private Action onComplete;
        private Action onStop;
        private Action onContinue;
        private Action onInterval;

        //限制构造，只能通过TimerManager创建
        private FTimer()
        { }
        internal FTimer(float duration)
        {
            this.duration = duration;
            isRunning = false;
            isActive = false;
            isCompleted = false;

            //设置默认曲线为正斜率
            curve = AnimationCurve.Linear(0, 0, 1, 1);
            FTM.AddTimer(this);
        }

        public void Dispose()
        {
            // 释放资源的逻辑
            isRunning = false;
            isActive = false;
            isCompleted = false;
            curve = null;

            // 清除所有的回调
            onUpdate = null;
            onCompleteUpdate = null;
            onStart = null;
            onComplete = null;
            onStop = null;
            onContinue = null;
            onInterval = null;
        }

        //(关闭)重置计时器(停止事件触发)
        public FTimer Close()
        {
            if (isActive)
            {
                endPoint = Time.time + duration;
                startPoint = Time.time;

                isActive = false;
                isRunning = false;
            }
            return this;
        }

        //启动计时器
        public FTimer Start()
        {
            if (!isActive)
            {
                startPoint = Time.time;
                endPoint = startPoint + duration;
                currentIntervalIndex = 1;

                isRunning = true;
                isCompleted = false;
                isActive = true;
                onStart?.Invoke();
            }
            return this;
        }

        //暂停计时(并停止事件触发)
        public FTimer Stop()
        {
            if (isRunning && isActive)
            {
                isRunning = false;
                //记录停止时间点
                stopPoint = Time.time;

                onStop?.Invoke();
            }
            return this;
        }

        //继续计时(如果结束了则会重新开始)
        public FTimer Continue()
        {
            if (!isRunning && isActive)
            {
                isRunning = true;
                startPoint += Time.time - stopPoint;
                endPoint += Time.time - stopPoint;

                onContinue?.Invoke();
            }
            return this;
        }
        //循环
        public FTimer SetLoop(int numbers)
        {
            LoopMAX = Mathf.Clamp(numbers, 1, numbers);
            return this;
        }
        //重置已循环次数
        public FTimer ResetLoop()
        {
            LoopCUR = 1;
            return this;
        }

        //反向时间(剩余时间)
        public float GetResultTime()
        {
            return isRunning ? Mathf.Clamp(endPoint - Time.time, 0, duration)
                           : Mathf.Clamp(endPoint - stopPoint, 0, duration);
        }

        //正向时间(进行时间)
        public float GetForwardTime()
        {
            return isRunning ? Mathf.Clamp(Time.time - startPoint, 0, duration)
                       : Mathf.Clamp(stopPoint - startPoint, 0, duration);
        }

        //开始回调
        public FTimer OnStart(Action action)
        {
            onStart = action;
            return this;
        }

        //持续回调事件
        public FTimer OnUpdate(Action action)
        {
            onUpdate = action;
            return this;
        }

        //结束回调
        public FTimer OnComplete(Action action)
        {
            onComplete = action;
            return this;
        }

        //结束持续回调
        public FTimer OnCompleteUpdate(Action action)
        {
            onCompleteUpdate = action;
            return this;
        }

        //暂停回调
        public FTimer OnStop(Action action)
        {
            onStop = action;
            return this;
        }

        //恢复暂停回调
        public FTimer OnContinue(Action action)
        {
            onContinue = action;
            return this;
        }

        //间隔回调
        public FTimer OnInterval(int nums, AnimationCurve curve, Action action)
        {
            if (curve != null)
                this.curve = curve;

            intervalNum = nums;
            onInterval = action;
            return this;
        }
        public FTimer OnInterval(int nums, Action action)
        {
            intervalNum = nums;
            onInterval = action;
            return this;
        }

        public void UpdateTimer()
        {
            if (isActive)
            {
                if (isRunning)
                {
                    if (GetForwardTime() < duration)
                    {
                        onUpdate?.Invoke();

                        // 间隔事件处理
                        if (intervalNum > 0 && curve != null)
                        {
                            float elapsedTime = GetForwardTime();
                            float intervalTime = duration * curve.Evaluate((float)currentIntervalIndex / (intervalNum + 1));

                            if (elapsedTime >= intervalTime)
                            {
                                onInterval.Invoke();
                                currentIntervalIndex++;
                            }
                        }
                    }

                    if (GetForwardTime() == duration)
                    {
                        if (!isCompleted)
                        {
                            isCompleted = true;
                            onComplete?.Invoke();
                        }

                        if (LoopMAX == LoopCUR)
                            onCompleteUpdate?.Invoke();
                    }
                }
            }

            if (LoopMAX < 0 && isCompleted)
            {
                Close();
                Start();
            }
            if (LoopMAX > 0 && isCompleted)
            {
                if (LoopCUR < LoopMAX)
                {
                    LoopCUR++;
                    Close();
                    Start();
                }
            }

        }
    }
}
