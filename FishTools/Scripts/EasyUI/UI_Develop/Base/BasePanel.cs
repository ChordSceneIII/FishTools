using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 要使用EasyUI的Panel预制体必须挂载BasePanel类或者其派生类
/// </summary>
namespace EasyUI
{
    public abstract class BasePanel : MonoBehaviour
    {
        protected bool isRemove = false;

        [SerializeField, HideInInspector] internal string panelname;

        internal void Close()
        {
            isRemove = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}