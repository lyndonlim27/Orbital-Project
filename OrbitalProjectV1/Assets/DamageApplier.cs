using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageApplier : MonoBehaviour
{
    protected DetectionScript detectionScript;
    protected EntityBehaviour parent;
    protected EnemyData enemyData;
    protected TrapData trapData;
    protected float attackSpeed;
    protected int damage;
    protected bool damaging;

    private void Awake()
    {
        CheckingParent();
    }

    private void CheckingParent()
    {
        parent = transform.parent.GetComponent<EntityBehaviour>();
        enemyData = parent.GetData() as EnemyData;
        trapData = parent.GetData() as TrapData;
        SettingUpDamageVals();
    }

    private void SettingUpDamageVals()
    {
        if (enemyData != null)
        {
            attackSpeed = enemyData.attackSpeed;
            damage = enemyData.damageValue;
        }
        else if (trapData != null)
        {
            attackSpeed = 1f;
            damage = trapData.damage;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player target = collision.gameObject.GetComponent<Player>();
        if (target != null && parent.inAnimation && !parent.isDead)
        {

            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(damage);
        }
    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player target = collision.gameObject.GetComponent<Player>();
        if (target != null && parent.inAnimation && !parent.isDead && !damaging) 
        {
            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            StartCoroutine(DoDamage(target));
            //target.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);
            
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
    }

    private IEnumerator DoDamage(Player player)
    {
        damaging = true;
        while(player != null && parent.inAnimation && !parent.isDead)
        {
            player.TakeDamage(damage);
            yield return new WaitForSeconds(1f);
        }
        damaging = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
