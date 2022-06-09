using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageApplier : MonoBehaviour
{
    protected DetectionScript detectionScript;
    protected EntityBehaviour parent;
    protected EnemyData enemyData;
    protected SwitchData switchData;
    protected float attackSpeed;
    protected int damage;

    private void Awake()
    {
        CheckingParent();
        //if (transform.parent.GetComponent<EnemyBehaviour>() != null)
        //{
        //    parent = transform.parent.GetComponent<EnemyBehaviour>();
        //} else
        //{
        //    parent = transform.parent.GetComponent<TrapBehaviour>();
        //}
        //enemyData = parent == null ? null : parent.enemyData;
    }

    private void CheckingParent()
    {
        parent = transform.parent.GetComponent<EntityBehaviour>();
        enemyData = parent.GetData() as EnemyData;
        switchData = parent.GetData() as SwitchData;
        SettingUpDamageVals();
    }

    private void SettingUpDamageVals()
    {
        if (enemyData != null)
        {
            attackSpeed = enemyData.attackSpeed;
            damage = enemyData.damageValue;
        }
        else if (switchData != null)
        {
            attackSpeed = 1f;
            damage = switchData.damage;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player target = collision.gameObject.GetComponent<Player>();
        if (target != null && parent.inAnimation)
        {

            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(damage);
        }
    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player target = collision.gameObject.GetComponent<Player>();
        if (target != null && parent.inAnimation)
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
        while(player != null && parent.inAnimation)
        {
            player.TakeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
