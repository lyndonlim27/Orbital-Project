using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class RoamState : StateClass
{
    public RoamState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    
    }

    public override void Enter(object data)
    {

        entity.animator.SetBool("isWalking", true);
        Roam();

    }

    public override void Update()
    {
        Roam();
    }

    public void Roam()
    {

        if (entity.detectionScript.playerDetected)
        {
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);

        } else if (entity.isReached())

        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
          else
        {
            entity.moveToRoam();
        }
     }
  

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
