using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackComponent
{ 

    public void Attack()

    {
        if (detectionScript.playerDetected)
        {
            if (target.IsDead())
            {
                return;
            }

            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(enemyData.damageValue);
        }
    }

}
