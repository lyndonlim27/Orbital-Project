using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderRanged : EnemyBehaviour
{
    public override void Start()
    {
        base.Start();
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new C_MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
    }

    public override void resetCooldown()
    {
        cooldown = 2f;
        inAnimation = false;
        stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

    }
}
