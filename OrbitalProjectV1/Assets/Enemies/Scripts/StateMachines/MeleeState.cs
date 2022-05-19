using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MeleeState : StateClass
{
    Player player;

    public MeleeState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter(object stateData)
    {
        tryAttack();
        
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
        
    }

    private void tryAttack()
    {
        Debug.Log("ATTACK");
        GameObject go = entity.melee.detectionScript.playerDetected;
        if (go == null)
        {
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
        }
        else
        {
            player = go.GetComponent<Player>();
            if (player.isDead())
            {
                Debug.Log("DEAD");
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            } else
            {
                entity.animator.SetTrigger("Melee");
            }
        }
    }

}

//    WeaponScript weapon;
//    Animator animator;
//    public override void Enter(object data)
//    {
//        animator = gameObject.GetComponent<Animator>();
//        MyEnemy enemyScript = gameObject.GetComponent<MyEnemy>();
//        weapon = gameObject.GetComponent<WeaponScript>();
//        EnemySpell spellPrefab = enemyScript.spellprefab;

//        int i = Random.Range(0, 2);
//        if (i == 0)
//            animator.SetTrigger("AttackTrigger");
//        else
//        {
//            animator.SetTrigger("CastTrigger");
//            WeaponScript weap = weapon.GetComponent<WeaponScript>();
//            GameObject target = weap.playerDetected;
//            if (target == null)
//            {
//                return;
//            }
//            EnemySpell enemySpell = GameObject.Instantiate(spellPrefab, target.transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
//        }
//    }
//    public override void Update()
//    {

//    }

//    public override void Exit()
//    {
//    }
//}