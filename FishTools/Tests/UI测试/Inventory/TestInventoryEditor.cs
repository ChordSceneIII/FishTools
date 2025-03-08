#if UNITY_EDITOR
using FishTools.EasyUI;
using FishToolsDEMO;
using UnityEditor;
using UnityEngine;

namespace FishTools.Tests
{

    [CustomEditor(typeof(TestInventory))]
    public class InventoryTestEditor : Editor
    {
        bool isFold = false;
        TestType propType;
        TestType propType2;
        int propAmount = 1;
        TestInventory bag;

        private void OnEnable()
        {
            bag = (TestInventory)target;
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            TestInventory inventory = (TestInventory)target;

            isFold = EditorGUILayout.Foldout(isFold, "编辑器工具");

            if (!isFold)
                return;

            if (GUILayout.Button("更新缓存"))
            {
                inventory.Cache();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("添加格子"))
            {
                inventory.AddSlot();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("移除格子"))
            {
                inventory.RemoveSlot();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("清空背包物品"))
            {
                inventory.ClearItem();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("刷新背包"))
            {
                inventory.ReloadCache();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("添加道具");
            propType = (TestType)EditorGUILayout.EnumPopup("类型", propType);
            propAmount = EditorGUILayout.IntField("数量", propAmount);
            if (GUILayout.Button("添加")) // 设置按钮宽度
            {
                bag.Add(propType, propAmount);
                EditorUtility.SetDirty(target);

            }

            GUILayout.Label("移除道具");
            propType2 = (TestType)EditorGUILayout.EnumPopup("类型", propType2);
            if (GUILayout.Button("移除")) // 设置按钮宽度
            {
                bag.Remove(propType2);
                EditorUtility.SetDirty(target);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
#endif