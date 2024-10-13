using System;
using System.Collections.Generic;
using EasyUI;
using FishToolsEditor;
using UnityEngine;

namespace EasyUI
{
    public abstract class BaseItemType<T> where T : Enum
    {
        public T value;
        public abstract GameObject GetPrefab();
        protected virtual GameObject GetPrefabDefault(Dictionary<T, GameObject> dic)
        {
            dic.TryGetValue(value, out var obj);

            if (obj == null)
            {
                DebugEditor.LogWarning("未找到类型对应的Item实例,请检查Type路径是否正确");
            }

            return obj;

        }
    }
}
///
/// /// /// /// /// 使用例
///

// public enum WeaponEnum
// {
//     Sword,
//     Bow,
//     Gun
// }

// public class TypeWeapon : BaseItemType<WeaponEnum>
// {
//     private static readonly Dictionary<WeaponEnum, GameObject> pathDictionary;

//     //懒加载
//     public static Dictionary<WeaponEnum, GameObject> PathDictionary
//     {
//         get
//         {
//             if (pathDictionary == null)
//             {
//                 return new Dictionary<WeaponEnum, GameObject>{
//                 { WeaponEnum.Sword, Resources.Load<GameObject>("Prefabs/Weapons/Sword") },
//                 { WeaponEnum.Bow,Resources.Load<GameObject>("Prefabs/Weapons/Bow")},
//                 { WeaponEnum.Gun,Resources.Load<GameObject>("Prefabs/Weapons/Gun")} };
//             }
//             return pathDictionary;
//         }

//     }
//     public override GameObject GetObj()
//     {
//         return GetObjDefault(PathDictionary);
//     }
// }