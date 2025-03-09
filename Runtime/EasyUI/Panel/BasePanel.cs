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
            PanelOperation.Instance.Repeat(gameObject);
        }
        public virtual void Open()
        {
            PanelOperation.Instance.Open(gameObject);
        }
        public virtual void Close()
        {
            PanelOperation.Instance.Close(gameObject);
        }
    }
}
