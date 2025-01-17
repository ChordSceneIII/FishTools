using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public static T FindByComponent<T>(string name, bool includeInactive = false) where T : Component
        {
            return GameObject.FindObjectsOfType<T>(includeInactive).FirstOrDefault(obj => obj.name == name);
        }

        /// <summary>
        /// @判断两个Rect物体的相对位置关系()
        /// </summary>
        public static Vector2 CompareRectDirection(RectTransform child, RectTransform parent, bool overflow = false)
        {
            Vector2 direction = Vector2.zero;

            Vector2 childMin = child.TransformPoint(child.rect.min);
            Vector2 childMax = child.TransformPoint(child.rect.max);
            Vector2 parentMin = parent.TransformPoint(parent.rect.min);
            Vector2 parentMax = parent.TransformPoint(parent.rect.max);

            if (childMax.x < parentMin.x)
            {
                direction.x = -1;
            }
            else if (overflow && childMin.x < parentMin.x)
            {
                direction.x = -1;
            }

            if (childMin.x > parentMax.x)
            {
                direction.x = 1;
            }
            else if (overflow && childMax.x > parentMax.x)
            {
                direction.x = 1;
            }

            if (childMax.y < parentMin.y)
            {
                direction.y = -1;
            }
            else if (overflow && childMin.y < parentMin.y)
            {
                direction.y = -1;
            }

            if (childMin.y > parentMax.y)
            {
                direction.y = 1;
            }
            else if (overflow && childMax.y > parentMax.y)
            {
                direction.y = 1;
            }

            return direction;
        }

        /// <summary>
        /// @条件监视器
        /// </summary>
        public static void Set_FMonitor(Func<bool> condition, Action action)
        {
            if (condition == null || action == null)
            {
                Debug.LogWarning("条件或回调不能为空");
                return;
            }

            // 创建一个观察者对象

            var monitor = new GameObject().AddComponent<F_Monitor>();
            monitor.name = $"F_Monitor_{monitor.GetInstanceID()}";
            monitor.StartCoroutine(monitor.ConditionWatcher(condition, action));
        }

        /// <summary>
        /// @等待完全销毁的回调监视器
        /// </summary>
        public static void AfterDestory(GameObject obj, Action action)
        {
            if (action == null)
            {
                Debug.LogWarning("回调事件不能为空");
                return;
            }
            // 创建一个观察者对象
            var monitor = new GameObject().AddComponent<F_Monitor>();
            monitor.name = $"F_Monitor_{monitor.GetInstanceID()}";
            monitor.StartCoroutine(monitor.ConditionWatcher(() => obj == null, action));
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

    #region 工具Mono脚本

    // 观察者脚本，检测物体是否已满足条件
    public sealed class F_Monitor : MonoBehaviour
    {

        // 通用条件监控：监控条件是否满足并触发回调
        public IEnumerator ConditionWatcher(Func<bool> condition, Action onConditionMet)
        {
            // 等待条件满足
            while (!condition())
            {
                yield return null;
            }

            // 条件满足后执行回调
            onConditionMet?.Invoke();

            // 销毁监视器对象自身
            Destroy(gameObject);
        }
    }
    #endregion

    #region 工具枚举

    #endregion
}