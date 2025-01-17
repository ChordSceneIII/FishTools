#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;


namespace FishTools.EasyUI
{
    [CustomEditor(typeof(Slot))]
    public class SlotEditor : ButtonEditor
    {
        SerializedProperty selectEvent;
        SerializedProperty itemObj;
        SerializedProperty selectKey;
        SerializedProperty islocked;
        Slot slot;

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEvent = serializedObject.FindProperty("selectEvent");
            itemObj = serializedObject.FindProperty("itemObj");
            selectKey = serializedObject.FindProperty("selectKey");
            islocked = serializedObject.FindProperty("islocked");
            slot = target as Slot;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(islocked);
            slot.IsLocked = slot.IsLocked;
            EditorGUILayout.PropertyField(itemObj);
            EditorGUILayout.PropertyField(selectKey);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(selectEvent);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif