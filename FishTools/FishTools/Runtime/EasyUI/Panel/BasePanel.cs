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
            FMonitor.Create().OnComplete(Open_);
        }
        public virtual void Close()
        {
            FMonitor.Create().OnComplete(Close_);
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
