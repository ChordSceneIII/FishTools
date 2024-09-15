using UnityEngine;
/// <summary>
/// 配置文件
/// </summary>

namespace SceneUITool
{
    public class SceneUIConfig : ScriptableObject
    {
        public LayerMask layerMask2D;
        public LayerMask layerMask3D;
        public float distance2D;
        public float distance3D;
    }
}
