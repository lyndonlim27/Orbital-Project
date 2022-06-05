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
        enemy.getNewRoamPosition();
        enemy.animator.SetBool("isWalking", true);

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

            Debug.Log("Dafuq");
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
          else
        {
            Debug.Log("Hmm");
            Debug.Log("enemy pos: " + enemy.transform.position);
            Debug.Log("roam pos: " + enemy.roamPos);
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
