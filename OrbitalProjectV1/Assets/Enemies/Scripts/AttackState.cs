using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AttackState : StateClass
{
    public AttackState(GameObject gameObject, StateMachine stateMachine) : base(gameObject, stateMachine) { }

    GameObject weapon;
    Animator animator;
    public override void Enter(object data)
    {
        animator = gameObject.GetComponent<Animator>();
        MyEnemy enemyScript = gameObject.GetComponent<MyEnemy>();
        weapon = enemyScript.customWeapon;
        EnemySpell spellPrefab = enemyScript.spellprefab;

        int i = Random.Range(0, 2);
        if (i == 0)
            animator.SetTrigger("AttackTrigger");
        else
        {
            animator.SetTrigger("CastTrigger");
            WeaponScript weap = weapon.GetComponent<WeaponScript>();
            GameObject target = weap.playerDetected;
            EnemySpell enemySpell = GameObject.Instantiate(spellPrefab, target.transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        }
    }
    public override void Update()
    {

    }

    public override void Exit()
    {
    }
}