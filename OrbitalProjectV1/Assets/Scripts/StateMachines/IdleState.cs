using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateClass
{
    private int counter;
    public IdleState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        enemy.animator.SetBool("isWalking", false);
        counter = 100;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        IdleCounter();
        
    }

    private void IdleCounter() {

        //let roam state handle detection of enemy. 
        if (counter == 0)
        {
            enemy.getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
            
        }
        else 
        {
            counter--;
        }

        enemy.tick();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
