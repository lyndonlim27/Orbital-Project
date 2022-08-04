using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class C_StopState : StateClass
    {
        public C_StopState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
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
            if (Vector2.Distance(enemy.transform.position, enemy.startingpos) <= 0f)
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