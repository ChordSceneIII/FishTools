using System;
using System.Collections.Generic;
using FishTools;
using UnityEngine;

namespace  FishTools.EasyUI
{
    public abstract class BaseItemType<T> where T : Enum
    {
        public T type;
        public abstract GameObject GetPrefab();
        protected virtual GameObject GetPrefabDefault(Dictionary<T, GameObject> dic)
        {
            dic.TryGetValue(type, out var obj);

            if (obj == null)
            {
                DebugF.LogWarning($"itemType: [ {type} ] 路径设置错误");
                return null;
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