using System;
using FishToolsEditor;
using UnityEngine;

/// <summary>
///  唯一ID实例,一个预制体对应唯一一个实例,使用Guid标识
/// </summary>

public sealed class UniquePrefab : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private string uniqueID;

    private UniquePrefab instance;

    void OnValidate()
    {
        // 如果在编辑器中尚未生成 uniqueID，则生成
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
            DebugEditor.Log($"{nameof(UniquePrefab)} 的 uniqueID 在编辑器中生成：{uniqueID}");
        }
    }

    void Start()
    {
        if (instance == null)
        {
            var instances = FindObjectsOfType<UniquePrefab>();
            foreach (var inst in instances)
            {
                if (inst.uniqueID == uniqueID && inst.instance != null)
                {
                    Destroy(this.gameObject);
                }
            }
            instance = this;
        }
    }

    public static GameObject GetInstance(UniquePrefab prefab)
    {
        try
        {
            //通过uniqueID获取唯一实例
            var instances = FindObjectsOfType<UniquePrefab>();
            foreach (var inst in instances)
            {
                if (inst?.uniqueID == prefab.uniqueID)
                {
                    DebugEditor.Log($"Unique找到对象,返回对象：{inst.gameObject}");
                    return inst.gameObject;
                }
            }
            DebugEditor.Log($"Unique未找到对象,新生成一个对象{prefab.gameObject}");
            return Instantiate(prefab.gameObject);
        }
        catch
        {
            return null;
        }
    }
}
