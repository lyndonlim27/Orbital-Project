using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateClass
{
    Player player;
    // store all detection scripts for casting and melee attacking;

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
        // if target in range of cast and not in range of melee, then cast.

        GameObject go = entity.detectionScript.playerDetected;

        if (go == null)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        } else
        {
            player = entity.detectionScript.playerDetected.GetComponent<Player>();
            Debug.Log(player);
            if (player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE,null);
            }
            else if (Vector2.Distance(entity.transform.position,entity.startingpos) >= entity.maxDist) 
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP,null);
            }
            else if (entity.inCastRange() && !entity.inMeleeRange() && !entity.onCooldown())
            {
                stateMachine.ChangeState(StateMachine.STATE.ATTACK2, null);
            }
            else if (entity.inMeleeRange())
            {
                stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
            } else
            {
                entity.moveToTarget(player);
            }
        }

        Debug.Log(stateMachine.currState);
    }

    

    

}
