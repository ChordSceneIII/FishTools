using UnityEngine;

namespace FishTools.SceneUI
{
    public abstract class BaseUIHandler : MonoBehaviour
    {
        protected virtual void Awake()
        {
            SceneUIManager.Instance.Initializer("SceneUIManager");
        }
    }
}