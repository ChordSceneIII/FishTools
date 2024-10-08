using SceneUITool;
using UnityEngine;

public abstract class BaseUIHandler : MonoBehaviour
{
    protected virtual void Awake()
    {
        SceneUIManager.Instance.Initializer("SceneUIManager");
    }
}