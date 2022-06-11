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

    private void triggerAttack()
    {

        //EliteMonsterA eliteMonsterA = (EliteMonsterA)enemy;
        //if (eliteMonsterA != null)
        //{
        //    if (eliteMonsterA.hpBarUI.HalfHP() && eliteMonsterA.HardenCooldown == 0)
        //    {
        //        stateMachine.ChangeState(StateMachine.STATE.RECOVERY, null);
        //    }

        //}

        if (enemy.player.IsDead())
        {
            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }
        else
        {
            if (!enemy.onCooldown())
            {
                List<string> rangedtriggers = enemy.enemyData.rangedtriggers;
                int random = Random.Range(0,enemy.enemyData.rangedtriggers.Count);
                enemy.flipFace(enemy.player.transform.position);
                enemy.animator.SetTrigger(rangedtriggers[random]);
                enemy.inAnimation = true;

            }
            else
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            }

        }

    }

    public override void Exit()
    {
    }


}
