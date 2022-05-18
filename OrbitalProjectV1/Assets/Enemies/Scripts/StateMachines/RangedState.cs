using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : StateClass
{
    Player player;

    public RangedState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
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

        Player player = entity.ranged.GetPlayer();
        //basically if player is not null or is not dead, we can initiate attack. for every other condition,
        // we just bounce back to chase state to decide.
        if (player == null)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

        }
        else
        {
            if (player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            } else
            {
                entity.ranged.Attack();
                entity.animator.SetTrigger("Cast");
            }
        }
    }

    public override void Exit()
    {
    }


}
