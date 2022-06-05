using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeState : StateClass
{
  
    public MeleeState(EnemyBehaviour enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter(object stateData)
    {
        triggerAttack();
    }

    public override void Update()
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

    

    private void triggerAttack()
    {

        EliteMonsterA eliteMonsterA = (EliteMonsterA)enemy;

        if (eliteMonsterA.hpBarUI.HalfHP() && eliteMonsterA.HardenCooldown == 0)
        {
            stateMachine.ChangeState(StateMachine.STATE.ENRAGED1, null);
        }

        //else if (!enemy.melee.detected())
        //{
        //    stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

        //}
        else if (enemy.player.isDead())
        {
            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }
        else
        {
            if (!enemy.inAnimation)
            {
                List<string> meleetriggers = enemy.enemyData.meleetriggers;
                int random = Random.Range(0, meleetriggers.Count);
                enemy.animator.SetTrigger(meleetriggers[random]);
                enemy.inAnimation = true;
                    
            } else
            {
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            }
                
        }
                       
    }
}


