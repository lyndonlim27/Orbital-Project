using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : StateClass
{
    public StopState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        returnToStartPos();
    }

    public override void Update()
    {
        returnToStartPos();
    }

    private void returnToStartPos()
    {
        if (Vector2.Distance(entity.transform.position,entity.startingpos) <= 0)
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        } else
        {
            entity.moveToStartPos();
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
