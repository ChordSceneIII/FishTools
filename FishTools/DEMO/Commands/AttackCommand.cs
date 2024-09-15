using UnityEngine;
using ActionSystem;

namespace FishTools
{

    [CreateAssetMenu(fileName = "AttackCommand", menuName = "ActionSystem/Command/AttackCommand")]
    public class AttackCommand : BaseCommand
    {
        SpriteRenderer spriteRenderer => PlayerEntity.Instance.GetComponent<SpriteRenderer>();
        public int Damage;
        protected override void OnExecute()
        {
            spriteRenderer.color = Color.red;
        }

        protected override void OnBreak()
        {
            spriteRenderer.color = PlayerEntity.Instance.oringalColor;
        }

        protected override void OnExit()
        {
            spriteRenderer.color = PlayerEntity.Instance.oringalColor;
        }

    }
}