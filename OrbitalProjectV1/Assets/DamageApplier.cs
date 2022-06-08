using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageApplier : MonoBehaviour
{
    protected DetectionScript detectionScript;
    protected EnemyBehaviour parent;
    protected EnemyData enemyData;

    private void Awake()
    {
        parent = transform.parent.GetComponent<EnemyBehaviour>();
        enemyData = parent.enemyData;
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player target = collision.gameObject.GetComponent<Player>();
        if (target != null && parent.inAnimation)
        {

            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            Debug.Log(target);
            Debug.Log(enemyData);
            target.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(enemyData.damageValue);
        }
    
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
