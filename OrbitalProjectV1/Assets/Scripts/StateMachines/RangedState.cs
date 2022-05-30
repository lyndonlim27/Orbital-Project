using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : StateClass
{
    public RangedState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter(object stateData)
    {

        enemy.animator.SetBool("isWalking", false);
        triggerAttack();  

    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
    }

    public void triggerAttack()
    {
        EliteMonsterA eliteMonsterA = (EliteMonsterA)enemy;

        if(eliteMonsterA.hpBarUI.HalfHP())
        {
            stateMachine.ChangeState(StateMachine.STATE.ENRAGED1, null);
        }

        else if (!enemy.ranged.detected())
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
            return;
        }

        else
        {
            if (!enemy.onCooldown())
            {
                enemy.ranged.Attack();
            } else
            {
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
            }
            
        }
    }

    public override void Exit()
    {
    }


}
