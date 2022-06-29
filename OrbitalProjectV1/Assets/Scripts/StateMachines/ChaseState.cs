using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the CONNECTING state to all other states.
//we can use this state to decide whether to do a ranged or melee attack, roam or chase or return to position.
public class ChaseState : StateClass
{
    public ChaseState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) {}
    public override void Enter(object stateData)
    {
        if (!enemy.animator.GetBool("isChasing"))
        {
            enemy.animator.SetBool("isChasing", true);
        }
        
        ChaseEnemy();
    }

    public override void Update()
    {
        ChaseEnemy();
        //for other logics while in the current state for future implementation;
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        enemy.animator.SetBool("isChasing", false);
    }

    public void ChaseEnemy()
    {

        //if object detected but is not player,
        if (!enemy.detectionScript.playerDetected)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }

        //else if (enemy.TravelToofar())
        //{
        //    stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        //}

        else
        {

            if (enemy.melee.isActiveAndEnabled && enemy.melee.detected())
            {

                stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
            }
            else if (enemy.ranged.isActiveAndEnabled && enemy.ranged.detected() && !enemy.onCooldown()) //&& enemy.CheckInsideRoom()
            {
                stateMachine.ChangeState(StateMachine.STATE.ATTACK2, null);
            }
          
            else
            {
                if (enemy.melee == null)
                {
                    stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
                } else
                {
                    enemy.moveToTarget(enemy.player);
                }
             
            } 

        }
               
    }
}

    

 
