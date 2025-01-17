using System.Collections.Generic;
using UnityEngine;

namespace FishTools.EasyUI
{
    /// <summary>
    /// 基础处理器（受到CanvasGroup及父级影响）
    /// </summary>
    public abstract class BaseHandler : MonoBehaviour
    {
        public bool interactable = true;

        public bool ingnoreCanvasGroup = false;
        private List<CanvasGroup> parentCanvasGroups = new List<CanvasGroup>();
        private CanvasGroup groupSelf;

        protected virtual void Start()
        {
            groupSelf = GetComponent<CanvasGroup>();
            CacheParentCanvasGroups();
            UpdateInteractableState();
        }

        protected virtual void Update()
        {
            UpdateInteractableState();
        }

        /// <summary>
        /// 缓存父级中的 CanvasGroup
        /// </summary>
        private void CacheParentCanvasGroups()
        {
            parentCanvasGroups.Clear(); // 清空缓存

            Transform current = transform.parent; // 从父级开始遍历
            while (current != null)
            {
                CanvasGroup canvasGroup = current.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    parentCanvasGroups.Add(canvasGroup); // 缓存父级 CanvasGroup
                }
                current = current.parent; // 继续向上遍历
            }
        }

        private void UpdateInteractableState()
        {
            // 如果 ingnoreCanvasGroup 为 true，则不检查 CanvasGroup 的状态
            if (ingnoreCanvasGroup == true)
                return;

            //groupSelf.interactable为false时返回false
            if (groupSelf != null && groupSelf.interactable == false)
            {
                interactable = false;
                return;
            }

            //groupself忽略父级
            if (groupSelf != null && groupSelf.ignoreParentGroups)
            {
                interactable = groupSelf.interactable;
                return;
            }

            //父级Group为空时返回
            if (parentCanvasGroups.Count == 0)
            {
                if (groupSelf != null) interactable = groupSelf.interactable;

                return;
            }

            //检查父级
            foreach (var canvasGroup in parentCanvasGroups)
            {
                // 如果父级 CanvasGroup 的 interactable 或 blocksRaycasts 为 false，则不可交互
                if (!canvasGroup.interactable || !canvasGroup.blocksRaycasts)
                {
                    interactable = false;
                    return;
                }
            }

            interactable = true; // 所有父级 CanvasGroup 的 interactable 和 blocksRaycasts 都为 true，可交互
        }

    }
}