using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_IdleState : StateClass
{
    private float counter;
    public S_IdleState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {

    }

    public override void Enter(object stateData)
    {
        counter = 1.5f;
        enemy.animator.SetBool("IsWalking", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        IdleCounter();
    }

    private void IdleCounter()
    {

        //let roam state handle detection of enemy. 
        if (counter <= 0)
        {
            enemy.getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

        }
        else
        {
            counter-=Time.deltaTime;
        }

        enemy.tick();
    }
}
