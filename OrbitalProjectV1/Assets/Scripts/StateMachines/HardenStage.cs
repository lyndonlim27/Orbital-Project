using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardenStage : StateClass
{
    public float regentimer;
    EliteMonsterA eliteMonster;
    EnemyData[] enemyDatas;
    public HardenStage(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
        
    }

    public override void Enter(object stateData)
    {
        eliteMonster = (EliteMonsterA)enemy;
        regentimer = 10;
        if (eliteMonster.allProps.Count == 0)
        {
            eliteMonster.ActivateStage1();
        }
        eliteMonster.animator.SetTrigger("Harden");
    }

    public override void Exit()
    {
        eliteMonster.DestroyFodders();
        eliteMonster.DestroyBossProps();
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {

        eliteMonster.CheckInteruption();
        if (regentimer <= 0)
        {
            eliteMonster.RegenHP();
            ResetRegenTimer();
            
        } else
        {
            regentimer -= Time.deltaTime;
        }
        
        
    }

    public void ResetRegenTimer()
    {
        regentimer = 5;
    }

}
