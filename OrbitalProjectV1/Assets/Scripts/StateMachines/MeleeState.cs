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

    

    private void triggerAttack()
    {

        EliteMonsterA eliteMonsterA = (EliteMonsterA)enemy;

        if (eliteMonsterA.hpBarUI.HalfHP() && eliteMonsterA.HardenCooldown == 0)
        {
            stateMachine.ChangeState(StateMachine.STATE.ENRAGED1, null);
        }

        else if (!enemy.melee.detected())
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);

        }
        else
        {
            if (enemy.player.isDead())
            {
                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
            }
            else
            {
                enemy.animator.SetTrigger("Melee");
                return;
            }
                //// slight difference from ranged, we should only apply the physics from melee attack upon
                //// the animated hit. so we add the meleeattack into the animation event of the melee animation.
                //if (enemy.hasWeapon())
                //{


                //    // right now idk how many weapons we can load, maybe can have more
                //    // maybe when we decided to add more weapons can have an array weapon[]
                //    int random = Random.Range(0, 2);
                //    Debug.Log(random);
                //    if (random == 0)
                //    {
                //        enemy.animator.SetTrigger("Melee");
                //        return;
                //    }
                //    else
                //    {
                //        enemy.animator.SetTrigger("WeaponAttack");
                //        return;
                //    }

                //}
                //else
                //{

            

            }
        }
    }


