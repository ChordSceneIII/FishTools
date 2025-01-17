#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FishTools.EasyUI
{
    [CustomEditor(typeof(FollowHandler))]
    public class FollowHandlerEditor : Editor
    {
        private FollowHandler followHandler;

        private void OnEnable()
        {
            followHandler = (FollowHandler)target;
            // 订阅 EditorApplication.update 事件
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            // 取消订阅 EditorApplication.update 事件
            EditorApplication.update -= OnEditorUpdate;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        /// <summary>
        /// 在编辑模式下每帧调用
        /// </summary>
        private void OnEditorUpdate()
        {
            if (followHandler != null && followHandler.smoothTime > 0)
            {
                followHandler.UpdatePosition();
            }
        }
    }
}
#endif