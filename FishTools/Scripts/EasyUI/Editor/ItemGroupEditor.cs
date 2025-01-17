#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace FishTools.EasyUI
{
    [CustomEditor(typeof(ItemGroup))]
    public class ItemGroupEditor : Editor
    {
        ItemGroup group;
        SerializedProperty size;
        SerializedProperty alwaysKeepGrid;

        void OnEnable()
        {
            group = target as ItemGroup;
            size = serializedObject.FindProperty("size");
            alwaysKeepGrid = serializedObject.FindProperty("alwaysKeepGrid");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(alwaysKeepGrid);
            EditorGUILayout.PropertyField(size);
            if (GUILayout.Button("更新布局"))
            {
                group.UpdateLayout();
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif