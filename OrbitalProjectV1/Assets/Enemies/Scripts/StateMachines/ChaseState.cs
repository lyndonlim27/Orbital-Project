using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the CONNECTING state to all other states.
//we can use this state to decide whether to do a ranged or melee attack, roam or chase or return to position.
public class ChaseState : StateClass
{
    Player player;
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
        GameObject go = entity.detectionScript.playerDetected;
        //if cant detect any objects
        Debug.Log("This is" + go);
        Debug.Log(entity.ranged.GetPlayer());
        Debug.Log(entity.melee.GetPlayer());
        if (go != null)
        {
            player = go.GetComponent<Player>();

        } else
        {
            player = null;
        }
            
        //if object detected but is not player, 
        if (player == null)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }
        else if (player.isDead())
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        }
        else if (Vector2.Distance(entity.transform.position, entity.startingpos) >= entity.maxDist)
        {
            Debug.Log("Stop");
            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }
        
        else if (entity.ranged.GetPlayer() != null && !entity.onCooldown())
        {
            stateMachine.ChangeState(StateMachine.STATE.ATTACK2, null);
            Debug.Log("Inside Attack2");
                
        } else if (entity.melee.GetPlayer() != null) {

            stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
            Debug.Log("Inside Attack1");
        } else
        {
            Debug.Log("Inside Chse");
            entity.moveToTarget(player);
        }
            
               
    }
}

    

 
