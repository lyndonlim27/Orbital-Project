using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class C_MeleeState : StateClass
{

    public C_MeleeState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter(object stateData)
    {
        triggerAttack();
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
        
    }

    private void triggerAttack()
    {

        if (!enemy.detectionScript.playerDetected)
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
        else
        {
            if (enemy.player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            }
            else
            {
                enemy.flipFace(enemy.player.transform.position);
                enemy.animator.SetTrigger("Melee");
                return;
            }
        }
    }
}