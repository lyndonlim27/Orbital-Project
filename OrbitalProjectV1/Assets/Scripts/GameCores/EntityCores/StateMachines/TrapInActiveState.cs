using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCores;

namespace EntityCores.StateMachines
{
    public class TrapInActiveState : StateClass
    {
        private float enteredTime;
        public TrapInActiveState(TrapBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
        {

        }

        public override void Enter(object stateData)
        {
            enteredTime = Time.time;

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
       
            bool canActivate = Time.time >= enteredTime + trap.trapData.duration;
            if (!trap.trapData.ontrigger)
            {
                if (canActivate)
                {
                    stateMachine.ChangeState(StateMachine.STATE.TRAPACTIVE, null);
                }
                else
                {
                    trap.animator.enabled = trap.inAnimation;
                    trap.spriteRenderer.sprite = null;
                }
            }
            else
            {
                if (trap.detectionScript.playerDetected)
                {
                    stateMachine.ChangeState(StateMachine.STATE.TRAPACTIVE, null);
                }
                else
                {
                    trap.animator.enabled = trap.inAnimation;
                    trap.spriteRenderer.sprite = null;
                }
            }


            //if (trap.trapData.ontrigger)
            //{
            //    if (trap.detectionScript.playerDetected)
            //    {
            //        stateMachine.ChangeState(StateMachine.STATE.TRAPACTIVE, null);
            //    } else
            //    {
            //        trap.animator.enabled = false;
            //        trap.spriteRenderer.sprite = null;
            //    }

            //}
            //else
            //{
            //    if (canActivate)
            //    {
            //        stateMachine.ChangeState(StateMachine.STATE.TRAPACTIVE, null);
            //    } else
            //    {
            //        trap.animator.enabled = false;
            //        trap.spriteRenderer.sprite = null;
            //    }

            //}
        }

        private void tick()
        {

        }
    }
}
