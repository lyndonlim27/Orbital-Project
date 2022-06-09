using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActiveState : StateClass
{
    private float activatedTime;
    public TrapActiveState(TrapBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {
        trap.animator.enabled = true;
        CheckForActivation();
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
        CheckIfTrapExpired();
        CheckForActivation();
    }

    private void CheckIfTrapExpired()
    {
        if (!trap.data.onetime)
        {
            if (Time.time >= activatedTime + trap.data.duration)
            {

                trap.animator.SetBool(trap.data.triggername, false);
                trap.inAnimation = false;
            }
        }
        
    }

    private void CheckForActivation()
    {
        if (!trap.detectionScript.playerDetected && !trap.inAnimation )
        {
            stateMachine.ChangeState(StateMachine.STATE.TRAPINACTIVE, null);
           
        } else if (!trap.inAnimation)
        {
            if (trap.data.onetime)
            {
                trap.inAnimation = true;
                trap.animator.SetTrigger(trap.data.triggername);
            }
            else
            {
                activatedTime = Time.time;
                trap.animator.SetBool(trap.data.triggername, true);
                
            }
            
        }
    }
}