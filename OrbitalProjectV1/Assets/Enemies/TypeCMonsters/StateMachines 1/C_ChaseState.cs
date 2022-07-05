using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the CONNECTING state to all other states.
//we can use this state to decide whether to do a ranged or melee attack, roam or chase or return to position.
public class C_ChaseState : StateClass
{
    public C_ChaseState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }
    public override void Enter(object stateData)
    {
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

    public void ChaseEnemy()
    {

        //if object detected but is not player, 
        if (!enemy.detectionScript.playerDetected)
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        }

        else if (Vector2.Distance(enemy.transform.position, enemy.startingpos) >= enemy.maxDist)
        {

            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }

        else if (enemy.ranged.detected() && !enemy.onCooldown())
        {
            stateMachine.ChangeState(StateMachine.STATE.ATTACK2, null);


        }
        else if (enemy.melee.detected())
        {

            stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);

        }
        else
        {
            enemy.MoveToTarget(enemy.player);

        }

        enemy.Tick();

    }
}