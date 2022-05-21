using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_IdleState : StateClass
{
    private int counter;
    public C_IdleState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        base.Enter(stateData);
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
        base.Update();
    }
}
