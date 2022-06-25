using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardenStage : StateClass
{
    public float regentimer;
    public HardenStage(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
        
    }

    public override void Enter(object stateData)
    {
        
        regentimer = 10;

    }

    

    public override void Exit()
    {
        

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {

        if (regentimer <= 0)
        {
            enemy.RegenHealth(5);
            ResetRegenTimer();
        } else
        {
            regentimer -= Time.deltaTime;
        }
        
        
    }

    public void ResetRegenTimer()
    {
        regentimer = 10;
    }

}
