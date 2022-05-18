using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General AttackState junction to decide the type of attacks.

public class AttackState : StateClass
{

    public AttackState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        CheckInRange();
    }

    public override void Update()
    {
        CheckInRange();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void CheckInRange()
    {
        if (entity.detectionScript.playerDetected == null)
        {
            //return back to original position
            entity.roamPos = entity.startingpos;
            entity.stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }
    }
}
