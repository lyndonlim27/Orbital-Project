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
        CheckForActivation();
    }

    private void CheckIfTrapExpired()
    {
        if (Time.time >= activatedTime + trap.trapData.duration)
        {

            stateMachine.ChangeState(StateMachine.STATE.TRAPINACTIVE, null);
        } 
    }

    private void CheckForActivation()
    {
        if (!trap.inAnimation)
        {
            if (!trap.trapData.ontrigger)
            {
                trap.inAnimation = true;
                trap.animator.SetTrigger(trap.trapData.triggername);
                stateMachine.ChangeState(StateMachine.STATE.TRAPINACTIVE, null);
            }
       
            else {
                if (trap.detectionScript.playerDetected)
                {
                    trap.inAnimation = true;
                    trap.animator.SetTrigger(trap.trapData.triggername);
                }
                else
                {
                    stateMachine.ChangeState(StateMachine.STATE.TRAPINACTIVE, null);
                }


            }
        }

    }     
    
}