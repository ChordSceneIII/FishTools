using System;
using UnityEngine;
using FishTools.ActionSystem;


namespace FishToolsDEMO
{
    [CreateAssetMenu(fileName = "RunCommand", menuName = "ActionSystem/Command/Runcommand")]
    public class RunCommand : BaseCommand
    {
        Rigidbody2D rig2d => PlayerEntity.Instance.rig2d;
        public float runSpeed;
        float moveAmount = 0;
        protected override void OnExecute()
        {
            moveAmount = Input.GetAxisRaw("Horizontal") * runSpeed;
            rig2d.velocity = new Vector2(moveAmount, rig2d.velocity.y);
        }

        protected override void OnBreak()
        {
            moveAmount = 0;
            rig2d.velocity = new Vector2(0, rig2d.velocity.y);
        }

        protected override void OnExit()
        {
        }

        void OnEnable()
        {
            RollCommand.SetRunActive += SetActive;
        }
    }
}