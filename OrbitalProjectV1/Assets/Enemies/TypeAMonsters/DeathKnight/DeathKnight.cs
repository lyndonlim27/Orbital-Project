using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnight : Entity
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        stateMachine.Init(StateMachine.STATE.IDLE, null);
    }
}
