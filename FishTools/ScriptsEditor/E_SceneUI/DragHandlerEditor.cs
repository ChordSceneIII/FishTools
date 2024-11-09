using UnityEditor;
using UnityEngine;

namespace FishTools.SceneUI
{
    [CustomEditor(typeof(DragHandler), true)]
    public class DragHandlerEditor : BaseUIHandlerEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
        }

    }
}