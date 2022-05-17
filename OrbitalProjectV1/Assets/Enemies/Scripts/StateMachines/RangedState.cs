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
        

    }

    public override void FixedUpdate()
    {
    }

    public void triggerAttack()
    {
        GameObject go = entity.ranged.detectionScript.playerDetected;
        if (go == null)
        {
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
        } else 
        {
            player = go.GetComponent<Player>();
            if (player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

            } else
            {
                entity.ranged.Attack(player);
                entity.resetCooldown();
            }
           
        }
    }

    public override void Exit()
    {
    }


}
