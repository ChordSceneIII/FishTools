using System.Collections;
using System.Collections.Generic;
using FishToolsEditor;
using UnityEngine;

/// <summary>
/// 要使用EasyUI的Panel预制体必须挂载BasePanel类或者其派生类
/// Panel类中不写控制UIPanel的方法
/// </summary>

namespace EasyUI
{
    public abstract class BasePanel : MonoBehaviour
    {
        protected bool isRemove = false;

        [SerializeField,ReadOnly]
        internal string panelname;

        internal void Close()
        {
            isRemove = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            panelname = this.name;
        }
    }
}
