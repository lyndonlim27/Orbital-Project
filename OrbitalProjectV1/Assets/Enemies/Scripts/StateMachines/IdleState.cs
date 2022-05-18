using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateClass
{
    private int counter = 100;
    public IdleState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        
        IdleCounter();
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
            entity.getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }
        else 
        {
            counter--;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
