using UnityEngine;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 默认Panel
    /// </summary>
    public class BasePanel : MonoBehaviour
    {
        RectTransform m_rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        public virtual void Repeat()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        public virtual void Open()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
        }
        public virtual void Close()
        {
            if (gameObject.activeSelf == true)
                gameObject.SetActive(false);
        }
    }

}
