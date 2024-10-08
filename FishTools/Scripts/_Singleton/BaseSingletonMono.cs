using UnityEngine;

/// <summary>
/// Mono自动单例
/// </summary>
namespace FishTools
{
    public class BaseSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                //查找是否已经存在单例
                if (!instance)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    //如果不存在则新创建一个单例
                    if (!instance)
                    {
                        GameObject obj = new GameObject(typeof(T).ToString());
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }


        //初始化，用于在游戏最开始的时候主动唤醒单例
        public virtual T Initializer()
        {
            return Instance;
        }

        public virtual T Initializer(string name)
        {
            Instance.gameObject.name =name;
            return Instance;
        }

        //预防重复创建单例,并销毁多余实例
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}