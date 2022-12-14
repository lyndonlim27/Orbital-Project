using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.StateMachines
{
    class C_MeleeState : StateClass
    {

        public C_MeleeState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

        public override void Enter(object stateData)
        {
            triggerAttack();
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

        }

        private void triggerAttack()
        {

            if (!enemy.detectionScript.playerDetected)
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

            }
            else
            {
                if (enemy.player.IsDead())
                {
                    stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                }
                else
                {
                    enemy.FlipFace(enemy.player.transform.position);
                    List<string> meleetriggers = enemy.enemyData.meleetriggers;
                    int random = Random.Range(0, meleetriggers.Count);
                    enemy.animator.SetTrigger(meleetriggers[random]);
                    return;
                }
               
            }
        }
    }
}