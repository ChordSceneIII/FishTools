using System;
using UnityEngine;
using ActionSystem;


namespace FishTools
{
    [CreateAssetMenu(fileName = "RollCommand", menuName = "ActionSystem/Command/RollCommand")]
    public class RollCommand : BaseCommand
    {
        Rigidbody2D rig2d => PlayerEntity.Instance.rig2d; public float impulse;

        protected override void OnExecute()
        {
            SetRunActive.Invoke(false);
            rig2d.AddForce(new Vector2(impulse, 0), ForceMode2D.Impulse);
        }
        protected override void OnBreak()
        {
        }

        protected override void OnExit()
        {
            SetRunActive.Invoke(true);
        }

        //控制RunCommand的激活状态
        public static event Action<bool> SetRunActive;
    }
}