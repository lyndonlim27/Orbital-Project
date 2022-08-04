using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCores.StateMachines;
namespace EntityCores.Enemy
{
    public class EliteMeleeFodder : EnemyBehaviour
    {
        // Start is called before the first frame update
        //private float UltCooldown;
        private void StartState()
        {
            stateMachine = new StateMachine();
            stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
            stateMachine.AddState(StateMachine.STATE.RECOVERY, new HardenStage(this, this.stateMachine));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StartState();
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        }

        //public void resetUltCooldown()
        //{
        //    UltCooldown = 30f;
        //    inAnimation = false;

        //}

    }
}