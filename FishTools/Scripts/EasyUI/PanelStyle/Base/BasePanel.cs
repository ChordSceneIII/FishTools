using UnityEngine;
using FishTools;

/// <summary>
/// 要使用EasyUI的Panel预制体必须挂载BasePanel类或者其派生类
/// Panel类中不写控制UIPanel的方法
/// </summary>

namespace FishTools.EasyUI
{
    public abstract class BasePanel : MonoBehaviour
    {
        [ReadOnly] public string rootCanvas;
        private Transform uiroot;
        public Transform UIRoot
        {
            get
            {
                if (uiroot == null)
                {
                    uiroot = GameObject.Find(rootCanvas)?.transform;
                    if (uiroot == null)
                    {
                        DebugF.LogError($"找不到Canvas:{rootCanvas}");
                    }
                }
                return uiroot;
            }
        }

    }
}
