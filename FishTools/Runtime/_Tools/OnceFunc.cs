using System;

namespace FishTools
{
    //单次执行类委托
    public struct OnceFunc
    {
        bool isExectued;

        //只允许执行一次
        public void DoOnly(Action action)
        {
            if (!isExectued)
            {
                action?.Invoke();
                isExectued = true;
            }
        }

        //允许执行N次
        public void DoMany(float times, Action action)
        {
            if (!isExectued)
            {
                for (int i = 0; i < times; i++)
                {
                    action?.Invoke();
                    if (i >= times - 1)
                    {
                        isExectued = true;
                    }
                }
            }
        }

        //在合适的周期(单次调用周期中)开放阀门
        public void UnLock()
        {
            isExectued = false;
        }
        public void Lock()
        {
            isExectued = true;
        }
    }
}

