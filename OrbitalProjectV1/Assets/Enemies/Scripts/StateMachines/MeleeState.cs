using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeState : StateClass
{
  
    public MeleeState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter(object stateData)
    {
        triggerAttack();
    }

    public override void Update()
    {
        //triggerAttack();

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
        if (!entity.melee.detected())
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

        }
        else 
        {
            if (entity.player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
            } else
            {
                // slight difference from ranged, we should only apply the physics from melee attack upon
                // the animated hit. so we add the meleeattack into the animation event of the melee animation.
                if (entity.hasWeapon())
                {


                    // right now idk how many weapons we can load, maybe can have more
                    // maybe when we decided to add more weapons can have an array weapon[]
                    int random = 1; //Random.Range(0, 2);
                    Debug.Log(random);
                    if (random == 0)
                    {
                        entity.animator.SetTrigger("Melee");
                    } else
                    {
                        entity.animator.SetTrigger("WeaponAttack");
                    }

                } else
                {
                    entity.animator.SetTrigger("Melee");
                }
                
            }
        }
    }

}

