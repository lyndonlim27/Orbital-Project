using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class StopState : StateClass
    {
        public StopState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }

        public override void Enter(object stateData)
        {
            returnToStartPos();
        }

        public override void Update()
        {
            returnToStartPos();
        }

        private void returnToStartPos()
        {
            //curr position > 5f from startingpos,
            if (enemy.detectionScript.playerDetected)
            {
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
            }

            else if (Vector2.Distance(enemy.transform.position, enemy.startingpos) <= 1f)
            {
                stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
            }
            else
            {
                enemy.MoveToStartPos();
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