using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FishTools.EasyUI
{
    [Flags]
    public enum Transtype
    {
        None = 0,
        Color = 1 << 0,
        Sprite = 1 << 1,
        Material = 1 << 2,
        Animation = 1 << 3,
    }

    /// <summary>
    /// 按键精灵,适用于2D,3D场景中的物体交互
    /// </summary>
    public class ButtonSprite : BaseHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public SpriteRenderer spriteRenderer;
        [Label("Key/Button")] public bool isKey = true;
        [ConField("isKey", true)] public KeyCode key;
        [ConField("isKey", false)] public string button;
        [Label("过渡效果")] public Transtype Ttype;
        [ConField("Ttype", Transtype.Color)] public Color originColor;
        [ConField("Ttype", Transtype.Color)] public Color hightLightColor;
        [ConField("Ttype", Transtype.Color)] public Color pressColor;
        [ConField("Ttype", Transtype.Color)] public Color unInteractColor;
        [ConField("Ttype", Transtype.Sprite)] public Sprite originSprite;
        [ConField("Ttype", Transtype.Sprite)] public Sprite hightLightSprite;
        [ConField("Ttype", Transtype.Sprite)] public Sprite pressSprite;
        [ConField("Ttype", Transtype.Sprite)] public Sprite unInteractSprite;
        [ConField("Ttype", Transtype.Material)] public Material originMaterial;
        [ConField("Ttype", Transtype.Material)] public Material hightLightMaterial;
        [ConField("Ttype", Transtype.Material)] public Material pressMaterial;
        [ConField("Ttype", Transtype.Material)] public Material unInteractMaterial;
        [ConField("Ttype", Transtype.Animation)] public Animation _animation;
        [ConField("Ttype", Transtype.Animation)] public AnimationClip originAnim;
        [ConField("Ttype", Transtype.Animation)] public AnimationClip hightLightAnim;
        [ConField("Ttype", Transtype.Animation)] public AnimationClip pressAnim;
        [ConField("Ttype", Transtype.Animation)] public AnimationClip unInteractAnim;
        public UnityEvent clickDownEvent;
        private bool isInside;

        private void OnEnable()
        {
            if (spriteRenderer == null)
            {
                DebugF.LogError("ButtonSprite: 请设置SpriteRenderer");
                return;
            }


            Recover();
        }

        protected override void Update()
        {
            base.Update();
            if (interactable)
            {
                //按下
                if ((isInside && Input.GetMouseButtonDown(0)) || (isKey && Input.GetKeyDown(key)) || (!isKey && Input.GetButtonDown(button)))
                {
                    Press();
                }

                if ((isInside && Input.GetMouseButtonUp(0)) || (isKey && Input.GetKeyUp(key)) || (!isKey && Input.GetButtonUp(button)))
                {
                    Recover();
                }

            }

        }

        protected override void OnInteractableChanged(bool interactable)
        {
            if (interactable)
                Recover();
            else
                UnInteract();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (interactable)
                HightLight();

            isInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (interactable)
                Recover();

            isInside = false;
        }

        private void Reset()
        {
            Ttype = Transtype.Color;

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originColor = spriteRenderer.color;
                originSprite = spriteRenderer.sprite;
                originMaterial = spriteRenderer.sharedMaterial;
                hightLightColor = FishUtility.DeltaHSV(originColor, 0f, -0.3f, 0.3f);
                pressColor = FishUtility.DeltaHSV(originColor, 0f, 0.3f, -0.3f);
                unInteractColor = FishUtility.DeltaHSV(originColor, 0f, -0.5f, -0.3f);
            }
        }

        private void HightLight()
        {
            if ((Ttype & Transtype.Color) != 0)
            {
                spriteRenderer.color = hightLightColor;
            }
            if ((Ttype & Transtype.Sprite) != 0)
            {
                spriteRenderer.sprite = hightLightSprite;
            }
            if ((Ttype & Transtype.Material) != 0)
            {
                spriteRenderer.material = hightLightMaterial;
            }
            if ((Ttype & Transtype.Animation) != 0 && _animation != null && hightLightAnim != null)
            {
                _animation.Play(hightLightAnim.name);
            }
        }

        private void Press()
        {
            clickDownEvent?.Invoke();

            if ((Ttype & Transtype.Color) != 0)
            {
                spriteRenderer.color = pressColor;
            }
            if ((Ttype & Transtype.Sprite) != 0)
            {
                spriteRenderer.sprite = pressSprite;
            }
            if ((Ttype & Transtype.Material) != 0)
            {
                spriteRenderer.material = pressMaterial;
            }
            if ((Ttype & Transtype.Animation) != 0 && _animation != null && pressAnim != null)
            {
                _animation.Play(pressAnim.name);
            }
        }

        private void Recover()
        {
            if ((Ttype & Transtype.Color) != 0)
            {
                spriteRenderer.color = originColor;
            }
            if ((Ttype & Transtype.Sprite) != 0)
            {
                spriteRenderer.sprite = originSprite;
            }
            if ((Ttype & Transtype.Material) != 0)
            {
                spriteRenderer.material = originMaterial;
            }
            if ((Ttype & Transtype.Animation) != 0 && _animation != null && originAnim != null)
            {
                _animation.Play(originAnim.name);
            }
        }

        private void UnInteract()
        {
            if ((Ttype & Transtype.Color) != 0)
            {
                spriteRenderer.color = unInteractColor;
            }
            if ((Ttype & Transtype.Sprite) != 0)
            {
                spriteRenderer.sprite = unInteractSprite;
            }
            if ((Ttype & Transtype.Material) != 0)
            {
                spriteRenderer.material = unInteractMaterial;
            }
            if ((Ttype & Transtype.Animation) != 0 && _animation != null && unInteractAnim != null)
            {
                _animation.Play(unInteractAnim.name);
            }
        }
    }
}