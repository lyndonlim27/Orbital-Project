using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class C_TeleportState : StateClass
    {
        public C_TeleportState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
        {

        }

        public override void Enter(object stateData)
        {
            enemy.animator.SetTrigger("Teleport");
            enemy.Teleport();

        }

        public override void Update()
        {


        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

    }
}