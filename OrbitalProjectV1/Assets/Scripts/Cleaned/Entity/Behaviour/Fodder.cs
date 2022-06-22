using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : EnemyBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
        stateMachine.Init(StateMachine.STATE.IDLE, null);
    }

    
}
