using System;
using UnityEngine;

/// <summary>
/// 子类请按照这样设置  [CreateAssetMenu(fileName = "child_type", menuName = "EayUI/ItemType/child_type_config", order = 1)]
/// </summary>

namespace FishTools.EasyUI
{
    public abstract class BaseItemPath<TYPE> : ScriptableObject where TYPE : Enum
    {
        public DictionarySer<TYPE, GameObject> typeDic;
        public bool TryGetPrefab(TYPE type, out GameObject prefab)
        {
            return typeDic.TryGetValue(type, out prefab);
        }
    }
}