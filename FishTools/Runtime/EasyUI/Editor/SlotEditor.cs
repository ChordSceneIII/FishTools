#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;


namespace FishTools.EasyUI
{
    [CustomEditor(typeof(Slot))]
    public class SlotEditor : ButtonEditor
    {
        SerializedProperty itemObj;
        SerializedProperty slectKey;
        SerializedProperty submitKey;
        SerializedProperty islocked;
        SerializedProperty lockedImage;
        SerializedProperty selectImage;
        Slot slot;
        private bool lastlokced;

        protected override void OnEnable()
        {
            base.OnEnable();
            itemObj = serializedObject.FindProperty("itemObj");
            slectKey = serializedObject.FindProperty("selectKey");
            submitKey = serializedObject.FindProperty("submitKey");
            islocked = serializedObject.FindProperty("islocked");
            lockedImage = serializedObject.FindProperty("lockedImage");
            selectImage = serializedObject.FindProperty("selectImage");
            slot = target as Slot;
            lastlokced = !slot.IsLocked;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(itemObj);
            EditorGUILayout.PropertyField(islocked);
            if (lastlokced != slot.IsLocked)
            {
                lastlokced = slot.IsLocked;
                slot.IsLocked = lastlokced;
            }
            EditorGUILayout.PropertyField(slectKey);
            EditorGUILayout.PropertyField(submitKey);
            EditorGUILayout.PropertyField(lockedImage);
            EditorGUILayout.PropertyField(selectImage);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
#endif