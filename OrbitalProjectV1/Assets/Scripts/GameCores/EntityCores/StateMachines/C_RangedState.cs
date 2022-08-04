using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class C_RangedState : StateClass
    {
        public C_RangedState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
        }

        public override void Enter(object stateData)
        {

            enemy.animator.SetBool("isWalking", false);
            triggerAttack();

        }

        public override void Update()
        {

        }

        public override void FixedUpdate()
        {
        }

        public void triggerAttack()
        {

            if (!enemy.ranged.detected())
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                return;
            }

            else
            {
                if (!enemy.onCooldown())
                {

                    List<string> rangedtriggers = enemy.enemyData.rangedtriggers;
                    int random = Random.Range(0, enemy.enemyData.rangedtriggers.Count);
                    enemy.animator.SetTrigger(rangedtriggers[random]);
                    enemy.inAnimation = true;
                }
                else
                {
                    if (enemy.enemyData.moveSpeed == 0)
                    {
                        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                        return;
                    }
                    else
                    {
                        stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                        return;
                    }

                }

            }
        }

        public override void Exit()
        {
        }


    }
}
