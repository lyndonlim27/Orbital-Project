using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class C_MeleeState : StateClass
{

    public C_MeleeState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter(object stateData)
    {
        base.Enter(stateData);
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
        base.Update();
    }
}