using UnityEngine;
using System;

/// <summary>
///  唯一ID实例,一个预制体对应唯一一个实例,使用Guid标识
/// </summary>

public sealed class UniquePrefab : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private string uniqueID ;

    private UniquePrefab instance ;
    void OnValidate()
    {
        // 如果在编辑器中尚未生成 uniqueID，则生成
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            Debug.Log($"{nameof(UniquePrefab)} 的 uniqueID 在编辑器中生成：{uniqueID}");
#endif
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
        //通过uniqueID获取唯一实例
        var instances = FindObjectsOfType<UniquePrefab>();
        foreach (var inst in instances)
        {
            if (inst.uniqueID == prefab.uniqueID)
            {
#if UNITY_EDITOR
                Debug.Log($"Unique找到对象,返回对象：{inst.gameObject}");
#endif
                return inst.gameObject;
            }
        }
#if UNITY_EDITOR
        Debug.Log($"Unique未找到对象,新生成一个对象{prefab.gameObject}");
#endif
        return Instantiate(prefab.gameObject);
    }

}
