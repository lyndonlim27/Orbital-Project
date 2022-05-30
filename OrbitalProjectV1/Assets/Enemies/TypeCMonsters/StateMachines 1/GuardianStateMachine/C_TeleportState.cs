using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_TeleportState : StateClass
{
    public C_TeleportState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {

    }
    private float teleportcounter = 3f;

    public override void Enter(object stateData)
    {
        enemy.animator.SetTrigger("Teleport");
        TeleportCounter();
        
    }

    public override void Update()
    {
        TeleportCounter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void TeleportCounter()
    {

        //let roam state handle detection of enemy if enemy is a non-stationary type.
        if (teleportcounter <= 0)
        {
            enemy.getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
        else
        {
            if (enemy.detectionScript.playerDetected && !enemy.onCooldown())
            {
                stateMachine.ChangeState(StateMachine.STATE.ATTACK1, null);
            }
            teleportcounter-= Time.deltaTime;

        }
        
        enemy.tick();
    }




}
