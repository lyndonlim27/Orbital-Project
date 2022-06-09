using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInActiveState : StateClass
{
    public TrapInActiveState(TrapBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {

    }

    public override void Enter(object stateData)
    {
        CheckIfActivated();
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
        CheckIfActivated();
    }

    private void CheckIfActivated()
    {
        if (trap.detectionScript.playerDetected && !trap.inAnimation)
        {
            stateMachine.ChangeState(StateMachine.STATE.TRAPACTIVE, null);
        }
        else
        {
            trap.animator.enabled = false;
        }
    }
}
