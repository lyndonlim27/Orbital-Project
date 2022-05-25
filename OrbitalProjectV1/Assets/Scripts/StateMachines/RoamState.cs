using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class RoamState : StateClass
{
    public RoamState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    
    }

    public override void Enter(object data)
    {

        enemy.animator.SetBool("isWalking", true);
        Roam();

    }

    public override void Update()
    {
        Roam();
    }

    public void Roam()
    {

        if (enemy.detectionScript.playerDetected)
        {
            
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);

        } else if (enemy.isReached())

        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
          else
        {
            enemy.moveToRoam();
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
