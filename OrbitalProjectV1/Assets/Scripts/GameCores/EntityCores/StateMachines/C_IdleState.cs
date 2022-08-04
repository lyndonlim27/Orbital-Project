using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class C_IdleState : StateClass
    {
        private float teleportcounter;
        public C_IdleState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {

        }

        public override void Enter(object stateData)
        {
            teleportcounter = 2f;
            //TeleportCounter();
        }


        private void TeleportCounter()
        {

            //let roam state handle detection of enemy if enemy is a non-stationary type.
            if (teleportcounter <= 0)
            {
                stateMachine.ChangeState(StateMachine.STATE.TELEPORT, null);

            }
            else
            {
                if (enemy.detectionScript.playerDetected)
                {
                    stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
                }

                teleportcounter -= Time.deltaTime;
            }

            enemy.Tick();
        }


        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            TeleportCounter();
        }
    }
}
