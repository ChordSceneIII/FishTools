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
        SerializedProperty parent;

        void OnEnable()
        {
            group = target as ItemGroup;
            size = serializedObject.FindProperty("size");
            alwaysKeepGrid = serializedObject.FindProperty("alwaysKeepGrid");
            parent = serializedObject.FindProperty("parent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(size);
            EditorGUILayout.PropertyField(alwaysKeepGrid);

            if (alwaysKeepGrid.boolValue == false && GUILayout.Button("更新布局"))
            {
                group.UpdateLayout();
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif