using UnityEngine;

/// <summary>
/// RectTransform 转成可序列化的数据工具
/// </summary>

namespace FishTools
{
    public static class RectTransformSerialize
    {
        // 将 RectTransform 转换为可序列化的数据结构
        public static RectTransformData FormRect(RectTransform rectTransform)
        {
            RectTransformData data = new RectTransformData();
            data.anchoredPosition = rectTransform.anchoredPosition;
            data.sizeDelta = rectTransform.sizeDelta;
            data.anchorMin = rectTransform.anchorMin;
            data.anchorMax = rectTransform.anchorMax;
            data.pivot = rectTransform.pivot;
            data.localRotation = rectTransform.localRotation;
            data.localScale = rectTransform.localScale;
            return data;
        }

        // 将可序列化的数据结构应用到 RectTransform
        public static void ToRect(RectTransform rectTransform, RectTransformData data)
        {
            rectTransform.anchoredPosition = data.anchoredPosition;
            rectTransform.sizeDelta = data.sizeDelta;
            rectTransform.anchorMin = data.anchorMin;
            rectTransform.anchorMax = data.anchorMax;
            rectTransform.pivot = data.pivot;
            rectTransform.localRotation = data.localRotation;
            rectTransform.localScale = data.localScale;
        }
    }

    [System.Serializable]
    public class RectTransformData
    {
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;
        public Quaternion localRotation;
        public Vector3 localScale;
    }
}
