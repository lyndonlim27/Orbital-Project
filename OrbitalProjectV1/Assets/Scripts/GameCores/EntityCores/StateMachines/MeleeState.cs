using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    public class MeleeState : StateClass
    {

        public MeleeState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

        public override void Enter(object stateData)
        {
            enemy.animator.SetBool("isChasing", false);
            enemy.animator.SetBool("isWalking", false);
            triggerAttack();
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



        private void triggerAttack()
        {

            if (enemy.player.IsDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
            }
            else
            {
                if (!enemy.inAnimation)
                {
                    List<string> meleetriggers = enemy.enemyData.meleetriggers;
                    enemy.LockMovement();
                    enemy.inAnimation = true;
                    int random = Random.Range(0, meleetriggers.Count);
                    enemy.animator.SetTrigger(meleetriggers[random]);

                }

              
            }

        }
    }
}


