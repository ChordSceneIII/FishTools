using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 默认Panel
    /// </summary>
    public class BasePanel : MonoBehaviour
    {
        RectTransform m_rectTransform;
        public RectTransform rectTransform => FishUtility.LazyGet(this, ref m_rectTransform);

        public virtual void Repeat()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        public virtual void Open()
        {
            Invoke("Open_", 0);//TOOD 暂时先用反射，后续用活跃单例来代替执行协程
        }
        public virtual void Close()
        {
            Invoke("Close_", 0);//TOOD 暂时先用反射，后续用活跃单例来代替执行协程
        }

        private void Open_()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
        }

        private void Close_()
        {
            if (gameObject.activeSelf == true)
                gameObject.SetActive(false);
        }

    }

}
