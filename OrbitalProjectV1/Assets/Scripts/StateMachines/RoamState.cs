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

    }

    public override void Update()
    {
        Roam();
        
        enemy.tick();
    }



    public void Roam()
    {

        if (enemy.detectionScript.playerDetected)
        {
            
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);

        } else if (enemy.isReached())

        {

            Debug.Log("Dafuq");
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
