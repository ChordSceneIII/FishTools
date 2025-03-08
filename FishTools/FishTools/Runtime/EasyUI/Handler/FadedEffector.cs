using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishTools.EasyUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadedEffector : BaseHandler
    {
        [SerializeField, ReadOnly] private CanvasGroup _group;
        public CanvasGroup group => FishUtility.LazyGet(this, ref _group);
        [SerializeField] private ObserveField<bool> isVisiable = new ObserveField<bool>(true);
        public bool IsVisiable => isVisiable.field;
        public float duration = 0.5f;
        public bool unscale = true;
        public FadeMode mode = FadeMode.Linear;

        void Awake()
        {
            isVisiable.AddListener(SetVisiable);
        }

        void OnDestroy()
        {
            isVisiable.RemoveListener(SetVisiable);
        }

        void OnEnable()
        {
            SetVisiable(isVisiable.field);
        }

        void OnDisable()
        {
            group.alpha = 0;
        }

        protected override void OnInteractableChanged(bool interactable)
        {
            if (interactable)
            {
                SetVisiable(true);
            }
            else
            {
                SetVisiable(false);
            }
        }

        public void SetVisiable(bool visiable)
        {
            if (visiable)
            {
                StartCoroutine(Show());
            }
            else
            {
                StartCoroutine(Hide());
            }
        }

        private IEnumerator Show()
        {
            float startAlpha = group.alpha;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += unscale ? Time.unscaledDeltaTime : Time.deltaTime;
                float progress = CalculateProgress(elapsedTime / duration, mode);
                group.alpha = Mathf.Lerp(startAlpha, 1, progress);
                yield return null;
            }

            group.alpha = 1;
        }

        private IEnumerator Hide()
        {
            float startAlpha = group.alpha;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += unscale ? Time.unscaledDeltaTime : Time.deltaTime;
                float progress = CalculateProgress(elapsedTime / duration, mode);
                group.alpha = Mathf.Lerp(startAlpha, 0, progress);
                yield return null;
            }

            group.alpha = 0;
        }

        private float CalculateProgress(float t, FadeMode mode)
        {
            switch (mode)
            {
                case FadeMode.Linear:
                    return t;
                case FadeMode.EaseIn:
                    return Mathf.Pow(t, 2);
                case FadeMode.EaseOut:
                    return Mathf.Sqrt(t);
                case FadeMode.EaseInOut:
                    return t < 0.5 ? Mathf.Pow(t, 2) : Mathf.Sqrt(1 - (1 - t) * 2);
                case FadeMode.Bounce:
                    return BounceEase(t);
                case FadeMode.EaseInBounce:
                    return BounceEase(1 - t);
                case FadeMode.EaseOutBounce:
                    return 1 - BounceEase(1 - t);
                default:
                    return t;
            }
        }

        private float BounceEase(float t)
        {
            const float n1 = 2.0272f;
            const float d1 = 1.5257f;

            if (t < 1 / d1)
            {
                return n1 * t;
            }
            else if (t < (1 + d1) / d1)
            {
                t -= 1 / d1;
                return n1 * t - n1 * (1 - d1) * t;
            }
            else if (t < (2 + d1) / d1)
            {
                t -= (1 + d1) / d1;
                return n1 * t + n1 * (d1 - 1) * t;
            }
            else
            {
                t -= (2 + d1) / d1;
                return n1 * t + n1 * (d1 - 1) * (1 - t);
            }
        }

        public enum FadeMode
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            Bounce,
            EaseInBounce,
            EaseOutBounce
        }
    }
}
