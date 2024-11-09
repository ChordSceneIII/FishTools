using UnityEditor;
using UnityEngine;
using FishTools.EasyUI;

namespace FishToolsEditor
{
    [CustomEditor(typeof(SlotSelectImage))]
    public class SlotSelectImageEditor : Editor
    {
        private SlotSelectImage SelectImage => (SlotSelectImage)target;
        private SlotGroup Group => SelectImage.GetComponent<SlotGroup>();
        public bool IsSelected
        {
            get
            {
                return Group.IsSelected;
            }
            set
            {
                Group.IsSelected = value;
            }
        }
        public override void OnInspectorGUI()
        {

            IsSelected = EditorGUILayout.Toggle("选择", IsSelected);

            DrawDefaultInspector();

            SelectImage._Init();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(SelectImage);
                EditorUtility.SetDirty(Group);
            }

        }
    }
}