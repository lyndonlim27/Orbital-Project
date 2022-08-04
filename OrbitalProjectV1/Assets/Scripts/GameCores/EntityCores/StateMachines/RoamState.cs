using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class RoamState : StateClass
    {
        public RoamState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {

        }

        public override void Enter(object data)
        {
            enemy.getNewRoamPosition();
            enemy.animator.SetBool("isWalking", true);

        }

        public override void Update()
        {
            Roam();
        }



        public void Roam()
        {

            if (enemy.detectionScript.playerDetected)
            {

                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);

            }
            else if (enemy.TravelToofar())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);

            }
            else if (enemy.IsReached())

            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

            }
            else
            {
                enemy.moveToRoam();
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
