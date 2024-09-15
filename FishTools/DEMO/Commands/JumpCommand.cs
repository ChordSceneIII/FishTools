using UnityEngine;
using ActionSystem;

namespace FishTools
{
    [CreateAssetMenu(fileName = "JumpCommand", menuName = "ActionSystem/Command/JumpCommand")]
    public class JumpCommand : BaseCommand
    {
        Rigidbody2D rig2d => PlayerEntity.Instance.rig2d;
        public float jumpSpeed;

        protected override void OnExecute()
        {
            rig2d.velocity = new Vector2(rig2d.velocity.x, jumpSpeed);
        }
        protected override void OnBreak()
        {
        }
        protected override void OnExit()
        {
        }
    }
}