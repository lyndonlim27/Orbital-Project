using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class RangedState : StateClass
    {
        public RangedState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
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

        private void triggerAttack()
        {
            if (enemy.player.IsDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
            }
            else
            {
                if (!enemy.onCooldown())
                {

                    List<string> rangedtriggers = enemy.enemyData.rangedtriggers;
                    List<string> dashattacks = enemy.enemyData.dashattacks;
                    int random = Random.Range(0, enemy.enemyData.rangedtriggers.Count);

                    enemy.FlipFace(enemy.player.transform.position);
                    enemy.animator.SetTrigger(rangedtriggers[random]);
                    enemy.inAnimation = true;
                    
                }

                else
                {
                    stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                }

            }

        }

        public override void Exit()
        {
        }


    }
}
