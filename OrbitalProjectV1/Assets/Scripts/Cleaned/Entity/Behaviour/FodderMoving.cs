using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderMoving : EnemyBehaviour
{
    // Start is called before the first frame update


    public override void Start()
    { 
        base.Start();
        transform.localScale = new Vector2(enemyData.scale, enemyData.scale);
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new C_MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        maxDist = 15f;
    }

    public override void resetCooldown()
    {
        cooldown = 2f;
        inAnimation = false;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

    }

}
