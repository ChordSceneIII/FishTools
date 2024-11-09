using UnityEditor;
using UnityEngine;

namespace FishTools.EasyUI
{
    [CustomEditor(typeof(SlotGroup))]
    public class SlotGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            SlotGroup slotGroup = (SlotGroup)target;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(label: "物体", obj: slotGroup.ItemObj, objType: typeof(GameObject), allowSceneObjects: true);
            EditorGUI.EndDisabledGroup();

            slotGroup.IsLocked = EditorGUILayout.Toggle("是否锁定", slotGroup.IsLocked);
            slotGroup.IsSelected = EditorGUILayout.Toggle("是否选中", slotGroup.IsSelected);
            slotGroup.itemSize = EditorGUILayout.Vector2Field("物体尺寸", slotGroup.itemSize);
            slotGroup.layer = EditorGUILayout.IntField("物体层级", slotGroup.layer);



            var over = slotGroup.GetComponent<SlotOverImage>();
            var select = slotGroup.GetComponent<SlotSelectImage>();

            if (over != null)
            {
                over._Init();
            }

            if (select != null)
            {
                select._Init();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(slotGroup);
                if (over != null)
                {
                    EditorUtility.SetDirty(over);
                }
                if (select != null)
                {
                    EditorUtility.SetDirty(select);
                }
            }

        }
    }
}