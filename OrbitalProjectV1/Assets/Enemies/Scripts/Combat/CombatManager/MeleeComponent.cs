using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackComponent
{
    public Weapon weapon;
    public DetectionScript detectionScript;

    public override void Init()
    {
        base.Init();
        weapon = GetComponent<Weapon>();
        detectionScript = GetComponent<DetectionScript>();
        
    }


    public override void Attack(Player target)

    {
        if (weapon == null) // if weap == null, means attack animation stored in parent animator.
        {
            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(damageValue);
            //weapon.animator.SetTrigger("Attack");
        } else
        {
            weapon.Attack();
        }

    }

    public bool inRange()
    {
        return detectionScript.playerDetected != null;
    }



}
