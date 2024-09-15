/// <summary>
/// 纯单例基类
/// </summary>
namespace FishTools
{
    public class BaseManager<T> where T : new()
    {
        private static T instance;
        private static readonly object locker = new object();
        public static T Instance
        {
            get
            {
                // 线程安全的延迟初始化
                lock (locker)
                {
                    instance ??= new T();
                }
                return instance;
            }
        }
    }
}