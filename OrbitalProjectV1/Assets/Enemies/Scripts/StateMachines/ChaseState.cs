using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the CONNECTING state to all other states.
//we can use this state to decide whether to do a ranged or melee attack, roam or chase or return to position.
public class ChaseState : StateClass
{
    public ChaseState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) {}
    public override void Enter(object stateData)
    {
        entity.animator.SetBool("isWalking", true);
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
    }

    public void ChaseEnemy()
    {
        
        //if object detected but is not player, 
        if (!entity.detectionScript.playerDetected)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }

        else if (Vector2.Distance(entity.transform.position, entity.startingpos) >= entity.maxDist)
        {

            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }
        
        else if (entity.ranged.detected() && !entity.onCooldown())
        {
            stateMachine.ChangeState(StateMachine.STATE.ATTACK2, null);

                
        } else if (entity.melee.detected()) {

            stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);

        } else
        {
            entity.moveToTarget(entity.player);
            
        }

        entity.tick();
               
    }
}

    

 
