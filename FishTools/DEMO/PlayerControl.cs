using UnityEngine;
using FishTools.ActionSystem;
using System;
using FishTools;

namespace FishToolsDEMO
{
    public class PlayerControl : BaseSingletonMono<PlayerControl>
    {
        Rigidbody2D rig2d => PlayerEntity.Instance.rig2d;
        RollCommand roll;
        JumpCommand jump;
        AttackCommand attack;
        RunCommand run;
        public ActionManager manager;

        protected override void Awake()
        {
            base.Awake();
            //读取配置文件
            manager = ActionManager.LoadAsset("actionData/ActionManager");

            run = manager.FindCommand<RunCommand>("run");
            jump = manager.FindCommand<JumpCommand>("jump");
            attack = manager.FindCommand<AttackCommand>("attack");
            roll = manager.FindCommand<RollCommand>("roll");

        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Z) && rig2d.velocity.y == 0)
            {
                manager.ExecuteCommand("attack");
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                attack.Exit();
            }

            if (Input.GetKeyDown(KeyCode.Space) && rig2d.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                manager.ExecuteCommand(jump);
                jump.Exit();
            }

            if (Input.GetKeyDown(KeyCode.C) && rig2d.velocity.y == 0 && roll.IsExecuting == false)
            {
                manager.ExecuteCommand(roll);
            }
            else if (roll.IsExecuting && rig2d.velocity.x == 0)
            {
                roll.Exit();
            }
        }

        bool isrun = false;
        void FixedUpdate()
        {
            //由于run是持续执行的，要打断run只能通过设置IsActive来，不然没有动作可以打断
            if (Input.GetAxis("Horizontal") != 0)
            {
                manager.ExecuteCommand(run);
                isrun = true;
            }
            else if (isrun == true)
            {
                isrun = false;
                run.Exit();
            }
        }
    }

}