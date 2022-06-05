using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_KnightState : StateClass
{
    private EliteMonsterS eliteMonsterS;
    public S_KnightState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
        eliteMonsterS = (EliteMonsterS)entity;
    }

    public override void Enter(object stateData)
    {
        eliteMonsterS.StartCoroutine(eliteMonsterS.triggerAttack((int) stateData));
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
        enemy.tick();
    }
}
