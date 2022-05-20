using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_StopState : StateClass
{
    public C_StopState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
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
        //curr position > 5f from startingpos,
        if (Vector2.Distance(entity.transform.position, entity.startingpos) <= 0f)
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
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