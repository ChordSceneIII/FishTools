using UnityEngine;

/// <summary>
/// Mono自动单例  (外部脚本不要再Awake中去拿单例Instance)
/// 不推荐在层级中主动去挂载该脚本,这类单例脚本适用于单例管理器中
/// </summary>
namespace FishTools
{
    public abstract class BaseSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
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
            Instance.gameObject.name = name;
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
        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}