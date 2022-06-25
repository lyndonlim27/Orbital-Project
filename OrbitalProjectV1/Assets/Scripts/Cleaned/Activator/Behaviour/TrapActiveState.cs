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
        //CheckIfTrapExpired();
        CheckForActivation();
    }

    private void CheckIfTrapExpired()
    {
        if (Time.time >= activatedTime + trap.trapData.duration)
        {

            //trap.animator.SetBool(trap.trapData.triggername, false);
            //trap.inAnimation = false;
            //trap.inAnimation = false;
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


                    //CheckIfTrapExpired();


                    //if (!trap.trapData.ontrigger)
                    //{
                    //    activatedTime = Time.time;
                    //    trap.inAnimation = true;
                    //    trap.animator.SetTrigger(trap.trapData.triggername);
                    //}

                    //else if (trap.detectionScript.playerDetected)
                    //{
                    //    trap.inAnimation = true;
                    //    trap.animator.SetTrigger(trap.trapData.triggername);

                    //activatedTime = Time.time;
                    //trap.animator.SetBool(trap.trapData.triggername, true);




            }
        }

    }     
    
}