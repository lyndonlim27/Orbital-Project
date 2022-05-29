using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_IdleState : StateClass
{
    private int teleportcounter;
    public C_IdleState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
        
    }

    public override void Enter(object stateData)
    {
        teleportcounter = 250;
        TeleportCounter();
    }


    private void TeleportCounter()
    {

        //let roam state handle detection of enemy if enemy is a non-stationary type.
        if (teleportcounter == 0)
        {
            enemy.getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.TELEPORT, null);

        }
        else
        {
            if (enemy.detectionScript.playerDetected && !enemy.onCooldown())
            {
                stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
            }

            teleportcounter--;
        }

        enemy.tick();
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
        TeleportCounter();
    }
}
