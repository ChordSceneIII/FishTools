using System.Collections.Generic;
using FishTools;

/// <summary>
/// 计时器触发器
///
/// 使用 Ftimer timer = FTM.Instance.CreateTimer(); 创建新的计时器
/// 使用   timer.Start(duration); 设置持续时间和启动方法
/// 使用   timer.Stop();        暂停计时器
/// 使用   timer.Continue();    继续计时器(和Stop一起使用，ReStart是回到开始时间)
/// 使用   timer.Rest()；       重置计时器，打断计时和所有方法,只有再执行Start才能启动计时器
///
/// </summary>
/// 触发时间(有独立的生命周期,由TimerManager管理)
///  OnUpdate(),OnCompleteUpdate(),OnStart(),OnStop(),OnContinue()
///  尽管这些回调事件的执行逻辑不会受Update等生命周期影响 但是 这些回调事件是覆盖委托，最好只设置一次
///
/// </summary>

namespace FishTools.TimeTool
{
    public class FTM : BaseSingletonMono<FTM>
    {
        internal List<FTimer> timerList = new List<FTimer>();
        public List<FTimer> TimerList => timerList;

        private void Update()
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                timerList[i].UpdateTimer();
            }
        }

        //把计时器加入管理列表中
        public void AddTimer(FTimer timer)
        {
            if (!timerList.Contains(timer))
            {
                timerList.Add(timer);
            }
        }

        //释放计时器
        public void RemoveTimer(FTimer timer)
        {
            if (timerList.Contains(timer))
            {
                timerList.Remove(timer);
            }
        }

        //如果有需要清理所有计时器的时候用
        public void ClearAll()
        {
            timerList.Clear();
        }
    }
}
