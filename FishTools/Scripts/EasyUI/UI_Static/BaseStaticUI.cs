using FishTools;
using UnityEngine;

/// <summary>
/// 静态UI面板，不会销毁而是是隐藏，在运行场景中一直存在
/// </summary>

namespace EasyUI
{
    public abstract class BaseStaticUI : MonoBehaviour
    {

        [SerializeField] internal string panelname;
    }
}
