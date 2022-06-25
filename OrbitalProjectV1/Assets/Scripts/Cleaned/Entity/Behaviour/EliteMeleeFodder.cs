using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMeleeFodder : EnemyBehaviour
{
    private float Teleportcooldown;
    private float UltCooldown;
    // Start is called before the first frame update
   
    private void StartState()
    {
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new C_MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.RECOVERY, new HardenStage(this, this.stateMachine));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //Reset stateMachine;
        StartState();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void resetUltCooldown()
    {

        UltCooldown = 30f;
        inAnimation = false;

    }

}