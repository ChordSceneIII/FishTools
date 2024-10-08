using System;
using System.Collections;
using System.Collections.Generic;
using FishToolsEditor;
using UnityEngine;

namespace EasyUI
{
    public abstract class BaseItem<DATA, TYPE> : MonoBehaviour, IDataToView where DATA : BaseItemDATA<TYPE> where TYPE : Enum
    {
        [Label("堆叠上限")] public int maxCount = 1;//最大数量。如果为1就是默认不叠加
        public DATA data;

        public int IMaxCount => maxCount;
        public int ICount
        {
            get
            {
                return data.Count;
            }
            set
            {
                data.Count = value;
            }
        }
        public Enum ITypeValue => data.Type.value;

    }


    ///
    ///使用例
    ///

    // public class WeaponItem : BaseItem<WeaponItemDATA, WeaponEnum>
    // {
    //     //这里拓展比如MAXCOUNT或者TAG之类的
    // }
}
