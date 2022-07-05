using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MageState : StateClass
{
    private EliteMonsterS eliteMonsterS;
    public S_MageState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
        eliteMonsterS = (EliteMonsterS)entity;
    }

    public override void Enter(object stateData)
    {
        eliteMonsterS.StartCoroutine(eliteMonsterS.triggerAttack((int)stateData));
        
    }

    //private void triggerAttack(int roll)
    //{
    //    if (enemy.player.isDead())
    //    {
    //        stateMachine.ChangeState(StateMachine.STATE.STOP, null);
    //    }
    //    else
    //    {   
    //        RangedComponent rangedComponent = enemy.ranged;
    //        rangedComponent.StartCoroutine(
    //            rangedComponent.Cast(rangedComponent.rangeds[roll-1]));
            
    //    }

    //}

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
