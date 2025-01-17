#if UNITY_EDITOR
using FishTools.EasyUI;
using FishToolsDEMO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestInventory))]
public class InventoryTestEditor : Editor
{
    bool isFold = false;
    TestTypeEnum propType;
    TestTypeEnum propType2;
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

        if (GUILayout.Button("更新数据"))
        {
            inventory.UpdateBag();
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
        if (GUILayout.Button("清空背包"))
        {
            inventory.ResetBag(inventory.data);
            EditorUtility.SetDirty(target);

        }
        if (GUILayout.Button("刷新背包"))
        {
            inventory.LoadALLDatas(inventory.data);
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.BeginVertical("box");

        GUILayout.Label("添加道具");
        propType = (TestTypeEnum)EditorGUILayout.EnumPopup("类型", propType);
        propAmount = EditorGUILayout.IntField("数量", propAmount);
        if (GUILayout.Button("添加")) // 设置按钮宽度
        {
            bag.Add(propType, propAmount);
            EditorUtility.SetDirty(target);

        }

        GUILayout.Label("移除道具");
        propType2 = (TestTypeEnum)EditorGUILayout.EnumPopup("类型", propType2);
        if (GUILayout.Button("移除")) // 设置按钮宽度
        {
            bag.Remove(propType2);
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndVertical();
    }
}
#endif