using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeState : StateClass
{
    Player player;

    public MeleeState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter(object stateData)
    {
        triggerAttack();
    }

    public override void Update()
    {
        //triggerAttack();

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
        Player player = entity.melee.GetPlayer();
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
                // slight difference from ranged, we should only apply the physics from melee attack upon
                // the animated hit. so we add the meleeattack into the animation event of the melee animation.
                entity.animator.SetTrigger("Melee");
            }
        }
    }

}

