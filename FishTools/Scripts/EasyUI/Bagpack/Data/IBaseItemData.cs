using System;
using UnityEngine;

namespace FishTools.EasyUI
{
    public interface IBaseItemData
    {
        GameObject gameObject { get; }
        int IMaxCount { get; }
        int ICount { get; set; }
        string SlotName { get; set; }
        Enum ITypeValue { get; }
    }
}