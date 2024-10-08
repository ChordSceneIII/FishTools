using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


namespace SceneUITool
{
    public class ButtonHandler : BaseUIHandler
    {
        internal enum TransitionType
        {
            None,
            Color,
            Sprite,
            Material,
            Animation
        }

        [SerializeField] internal TransitionType transitionType = TransitionType.None;

        [SerializeField] internal SpriteRenderer orignalSpriteRender;//原精灵渲染器


        [SerializeField] internal Animator animator; //动画器
        [SerializeField] internal AnimationClip i_AnimCLip;//待机动画

        [SerializeField] internal Sprite h_tSprite; //高亮精灵
        [SerializeField] internal Color h_Color = Color.white; //高亮颜色
        [SerializeField] internal Material h_Material; //高亮材质
        [SerializeField] internal AnimationClip h_AnimClip;//高亮动画

        [SerializeField] internal Sprite p_Sprite; //按下精灵
        [SerializeField] internal Color p_Color = Color.gray; //按下颜色
        [SerializeField] internal Material p_Material; //按下材质
        [SerializeField] internal AnimationClip p_AnimClip;//按下动画

        private Sprite originalSprite;
        private Color originalColor;
        private Material originalMaterial;

        internal bool isHighLight = true;


        private void OnEnable()
        {
            if (orignalSpriteRender != null)
            {
                originalSprite = orignalSpriteRender.sprite;
                originalColor = orignalSpriteRender.color;
                originalMaterial = orignalSpriteRender.material;
            }

        }

        #region 预设按钮状态变化 preset button state change
        //高亮显示
        public void HighLight()
        {
            if (orignalSpriteRender != null)
            {
                switch (transitionType)
                {
                    case TransitionType.Color:
                        orignalSpriteRender.color = h_Color;
                        break;
                    case TransitionType.Sprite:
                        if (h_tSprite != null)
                            orignalSpriteRender.sprite = h_tSprite;
                        break;
                    case TransitionType.Material:
                        if (h_Material != null)
                            orignalSpriteRender.material = h_Material;
                        break;
                }
            }
            if (animator != null)
            {
                switch (transitionType)
                {
                    case TransitionType.Animation:
                        if (h_AnimClip != null)
                            animator.Play(h_AnimClip.name);
                        break;
                }

            }
        }
        //按下显示
        public void Press()
        {
            if (orignalSpriteRender != null)
            {
                switch (transitionType)
                {
                    case TransitionType.Color:
                        orignalSpriteRender.color = p_Color;
                        break;
                    case TransitionType.Sprite:
                        if (p_Sprite != null)
                            orignalSpriteRender.sprite = p_Sprite;
                        break;
                    case TransitionType.Material:
                        if (p_Material != null)
                            orignalSpriteRender.material = p_Material;
                        break;
                }
            }
            if (animator != null)
            {
                switch (transitionType)
                {
                    case TransitionType.Animation:
                        if (p_AnimClip != null)
                            animator.Play(p_AnimClip.name);
                        break;
                }
            }
        }
        //恢复
        public void Recover()
        {
            if (orignalSpriteRender != null && transitionType == TransitionType.Color)
            {
                // 恢复为默认材质和颜色
                orignalSpriteRender.sprite = originalSprite;
                orignalSpriteRender.material = originalMaterial;
                orignalSpriteRender.color = originalColor;
            }

            if (orignalSpriteRender != null && transitionType == TransitionType.Sprite)
            {
                orignalSpriteRender.sprite = originalSprite;
            }

            if (animator != null && transitionType == TransitionType.Animation)
            {
                animator.Play(i_AnimCLip.name);//播放待机动画
            }
        }
        #endregion



        #region 事件处理 event handler
        [SerializeField]
        private UnityEvent onClickDown;

        public UnityEvent OnClickDown
        {
            get { return onClickDown; }
            set { onClickDown = value; }
        }

        [SerializeField]
        private UnityEvent onClickUp;

        public UnityEvent OnClickUp
        {
            get { return onClickUp; }
            set { onClickUp = value; }
        }

        [SerializeField]
        private UnityEvent onClick;

        public UnityEvent OnClick
        {
            get { return onClick; }
            set { onClick = value; }
        }
        [SerializeField]
        private UnityEvent onClickOutside;
        public UnityEvent OnClickOutside
        {
            get { return onClickOutside; }
            set { onClickOutside = value; }
        }

        [SerializeField]
        private UnityEvent onHoverOutside;
        public UnityEvent OnHoverOutside
        {
            get { return onHoverOutside; }
            set { onHoverOutside = value; }
        }

        [SerializeField]
        private UnityEvent onHoverInside;
        public UnityEvent OnHoverInside
        {
            get { return onHoverInside; }
            set { onHoverInside = value; }
        }

        public void HoverInsideEvent()
        {
            onHoverInside?.Invoke();
        }

        public void HoverOutsideEvent()
        {
            onHoverOutside?.Invoke();
        }

        public void ClickOutsideEvent()
        {
            onClickOutside?.Invoke();
        }

        public void ClickEvent()
        {
            onClick?.Invoke();
        }
        public void ClickDownEvent()
        {
            onClickDown?.Invoke();
        }

        public void ClickUpEvent()
        {
            onClickUp?.Invoke();
        }
        #endregion
    }
}