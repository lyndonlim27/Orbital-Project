using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : StateClass
{
    public RangedState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        
        triggerAttack();  

    }

    public override void Update()
    {
        //for other logics while in the current state for future implementation;
        //triggerAttack();

    }

    public override void FixedUpdate()
    {
    }

    public void triggerAttack()
    {
        //basically when in this state, we can initiate an attack already even if it misses.
        //However, if player is no longer in range in the next instance or dead, we want to go back to chase state and let it decide
        //the next logical move
        //basically if player is not null or is not dead, we can initiate attack. for every other condition,
        // we just bounce back to chase state to decide.
        Debug.Log("What?");
        if (!enemy.ranged.detected())
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
            return;
        }

        else
        {
            if (enemy.player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
                return;
            }
            else if (!enemy.onCooldown())
            {
                Debug.Log("We're in attack2!");
                enemy.animator.SetTrigger("Cast");
                enemy.ranged.Attack();
                return;
            } else
            {
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                return;
            }
        }
    }

    public override void Exit()
    {
    }


}
