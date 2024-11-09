using System;
using FishTools;
using UnityEngine;

namespace  FishTools.EasyUI
{
    [Serializable]
    public abstract class BaseItemDATA<T> where T : Enum
    {
        // public abstract BaseItemType<T> Type { get; set; }  //类型
        public abstract BaseItemType<T> Type { get; set; }
        [ReadOnly] public string slotName; //UI位置
        [SerializeField] protected int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                if (count < 0) count = 0;
            }
        }
    }

    ///
    ///使用例
    ///

    // public class WeaponItemDATA : BaseItemDATA<WeaponEnum>
    // {
    //     [SerializeField] private TypeWeapon type;
    //     public override BaseItemType<WeaponEnum> Type
    //     { get => type; set => type = (TypeWeapon)value; }
    // }
}
