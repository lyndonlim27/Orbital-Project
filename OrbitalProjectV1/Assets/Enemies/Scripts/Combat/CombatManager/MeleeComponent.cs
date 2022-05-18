using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackComponent
{
    public override void Attack()

    {
        if (detectionScript.playerDetected)
        {
            if (target.isDead())
            {
                return;
            }
            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * enemyStats.attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(enemyStats.damageValue);
        }
    }

}
