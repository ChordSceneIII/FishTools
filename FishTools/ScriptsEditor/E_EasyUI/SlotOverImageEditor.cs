using UnityEditor;
using UnityEngine;
using FishTools.EasyUI;

namespace FishToolsEditor
{
    [CustomEditor(typeof(SlotOverImage))]
    public class SlotOverImageEditor : Editor
    {
        private SlotOverImage OverImage => (SlotOverImage)target;
        private SlotGroup Group => OverImage.GetComponent<SlotGroup>();
        public bool IsLocked
        {
            get
            {
                return Group.IsLocked;
            }
            set
            {
                Group.IsLocked = value;
            }
        }
        public override void OnInspectorGUI()
        {

            IsLocked = EditorGUILayout.Toggle("锁定", IsLocked);

            DrawDefaultInspector();

            OverImage._Init();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(OverImage);
                EditorUtility.SetDirty(Group);
            }

        }
    }
}