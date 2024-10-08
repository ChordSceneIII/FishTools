using UnityEditor;
using UnityEngine;

namespace SceneUITool
{
    [CustomEditor(typeof(BaseUIHandler), true)]
    public class BaseUIHandlerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            BaseUIHandler handler = (BaseUIHandler)target;

            // 检查是否缺少 Collider 或 Collider2D
            Collider collider = handler.GetComponent<Collider>();
            Collider2D collider2D = handler.GetComponent<Collider2D>();

            // 如果两者都缺失，显示 HelpBox
            if (collider == null && collider2D == null)
            {
                EditorGUILayout.HelpBox("该组件需要一个Collider或Collider2D组件,否则无法使用", MessageType.Error);
            }
        }

    }
}