using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace FishTools
{

    #region 工具包
    /// <summary>
    /// 销毁相关的封装。用于外部对 将要销毁的对象的前后插入回调操作
    /// 用于弥补OnDestroy方法在游戏中延时执行的缺陷
    /// </summary>
    public static class FishUtility
    {
        /// <summary>
        /// 判断是否为null(空引用)或Null(丢失引用)
        /// </summary>
        public static bool IsNull(object item)
        {
            if (item == null || item.Equals(null))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>懒加载 (针对Component)</para>
        /// <para>type =0 self , =1: parent =2: children</para>
        /// </summary>
        public static T LazyGet<L, T>(L instance, ref T component, int type = 0) where T : Component where L : Component
        {
            if (FishUtility.IsNull(component))
            {
                if (instance != null)
                {
                    switch (type)
                    {
                        case 0:
                            component = instance.GetComponent<T>();
                            break;
                        case 1:
                            component = instance.GetComponentInParent<T>(true);
                            break;
                        case 2:
                            component = instance.GetComponentInChildren<T>(true);
                            break;
                    }
                }
                else
                {
                    DebugF.LogError("访问的是空实例");
                }
            }
            return component;
        }

        /// <summary>
        /// @替换旧对象为新对象，同时保持旧对象的位置和父级关系。
        /// </summary>
        public static void ReplaceGameobject(GameObject oldObject, GameObject newObject)
        {
            if (oldObject == null || newObject == null)
            {
                Debug.LogWarning("旧对象或新 prefab 为 null");
                return;
            }

            // 将新对象设为旧对象的父级
            oldObject.transform.SetParent(newObject.transform);

            // 销毁旧对象
            GameObject.Destroy(oldObject);
        }

        /// <summary>
        /// @在场景中寻找特定类型特定名字的物体
        /// </summary>
        public static T FindComponent<T>(string name, bool includeInactive = false) where T : Component
        {
            return GameObject.FindObjectsOfType<T>(includeInactive).FirstOrDefault(obj => obj.name == name);
        }

        /// <summary>
        /// @查找所有实现了接口的对象
        /// </summary>
        public static List<T> FindObjectsOfInterface<T>() where T : class
        {
            List<T> result = new List<T>();
            MonoBehaviour[] monos = GameObject.FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in monos)
            {
                if (mono is T)
                {
                    result.Add(mono as T);
                }
            }
            return result;
        }


        public static void DestoryGameobjects(Component[] components)
        {
            foreach (var obj in components)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }

        /// <summary>
        /// @销毁所有物体的游戏对象
        /// </summary>
        public static void DestroyAll<T>(List<T> objs) where T : Component
        {
            if (Application.isPlaying)
            {
                foreach (var _obj in objs)
                {
                    if (!FishUtility.IsNull(_obj))
                        GameObject.Destroy(_obj.gameObject);
                }
            }
            else
            {
                foreach (var _obj in objs)
                {
                    if (!FishUtility.IsNull(_obj))
                        GameObject.DestroyImmediate(_obj.gameObject);
                }
            }
        }

        /// <summary>
        /// @销毁所有物体的游戏对象
        /// </summary>
        public static void DestroyAll<T>(T[] objs) where T : Component
        {
            if (Application.isPlaying)
            {
                foreach (var _obj in objs)
                {
                    if (!FishUtility.IsNull(_obj))
                        GameObject.Destroy(_obj.gameObject);
                }
            }
            else
            {
                foreach (var _obj in objs)
                {
                    if (!FishUtility.IsNull(_obj))
                        GameObject.DestroyImmediate(_obj.gameObject);
                }
            }
        }

        public static void DestroyAllChilds(Transform parent)
        {
            if (Application.isPlaying)
            {
                foreach (Transform child in parent)
                {

                    GameObject.Destroy(child.gameObject);
                }
            }
            else
            {
                foreach (Transform child in parent)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }

        }

        /// <summary>
        /// @深拷贝，利用JSON序列化与反序列化，只对Serializable化的对象有效
        /// <summary>
        public static T DeepCopy<T>(T original)
        {
            // 直接序列化和反序列化进行深拷贝
            string json = JsonUtility.ToJson(original);
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// <para>@对给定的 Color 对象进行 HSV 调整。</para>
        /// <para>h：色相 s：饱和度 v：亮度</para>
        /// </summary>
        /// <param name="color">原始颜色。</param>
        /// <param name="h">色相调整量（-1 到 1）。</param>
        /// <param name="s">饱和度调整量（-1 到 1）。</param>
        /// <param name="v">亮度调整量（-1 到 1）。</param>
        /// <param name="a">透明度调整量（-1 到 1），默认值为 1。</param>
        public static Color DeltaHSV(Color color, float h, float s, float v, float a = 1f)
        {
            // 将 RGB 颜色转换为 HSV
            Color.RGBToHSV(color, out float originalH, out float originalS, out float originalV);

            // 调整色相、饱和度、亮度和透明度
            float newH = (originalH + h) % 1f; // 色相是循环的，使用取模运算
            float newS = Mathf.Clamp01(originalS + s); // 饱和度限制在 0 到 1
            float newV = Mathf.Clamp01(originalV + v); // 亮度限制在 0 到 1
            float newA = Mathf.Clamp01(color.a + a); // 透明度限制在 0 到 1

            // 将 HSV 转换回 RGB
            Color adjustedColor = Color.HSVToRGB(newH, newS, newV);

            // 设置透明度
            adjustedColor.a = newA;

            return adjustedColor;
        }

        /// <summary>
        /// @退出游戏
        /// </summary>
        public static void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// @切换场景
        /// </summary>
        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    #endregion
}