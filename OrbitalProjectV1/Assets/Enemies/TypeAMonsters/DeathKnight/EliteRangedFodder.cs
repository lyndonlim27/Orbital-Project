//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EliteRangedFodder : EnemyBehaviour
//{
//    // Start is called before the first frame update
//    public void Start()
//    {
        
//        stateMachine = new StateMachine();
//        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
//        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
//        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
//        stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
//        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
//        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
//        stateMachine.Init(StateMachine.STATE.IDLE, null);
//    }

//    public override void Update()
//    {
//        base.Update();
//    }

//    public override void resetCooldown()
//    {
//        cooldown = 5f;
//        inAnimation = false;


//    }

//    //public void resetUltCooldown()
//    //{
//    //    UltCooldown = 30f;
//    //    inAnimation = false;

//    //}

//    public override void Teleport()
//    {
//        getNewRoamPosition();
//        transform.parent.position = roamPos;
//        Dashcooldown = 30f;
//        inAnimation = false;
//        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
//    }

//}
