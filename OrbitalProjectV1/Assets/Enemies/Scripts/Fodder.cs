using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : Enemy
{
    private float nextAttackTime;
    public customWeapon customWeapon;
    public EnemySpell spellprefab;
    private float castTrigger;
    private float castCounter;


    private void Awake()
    {
        this.castTrigger = Random.Range(1f, 600f);
        this.castCounter = 0;
    }

    override

        public void Attack(Vector2 dir)
    {
        if (dir.x > 0)
        {
            customWeapon.AttackRight();
        }
        else
        {
            customWeapon.AttackLeft();
        }


    }

    private void Update()
    {
        if (target != null && castCounter >= castTrigger)
        {
            castTrigger = Rand();
            castCounter = 0;
            animator.SetTrigger("Casting");

        }
    }

    private void FixedUpdate()
    {
        float steps = speed * Time.fixedDeltaTime;
        float dist;
        Vector2 direction;
        if (target != null)
        {
            dist = Vector3.Distance(transform.position, target.position);
            direction = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
            if (dist < customWeapon.transform.localScale.x + 1f)
            {
                Attack(direction);
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, steps);
            }
            
            
        }
        else
        {
            dist = Vector3.Distance(transform.position, roamPosition);
            if (dist < nextWaypointDistance)
            {
                roamPosition = GetRoamingPosition();

            }
            transform.position = Vector2.MoveTowards(transform.position, roamPosition, steps);
            direction = ((Vector2)roamPosition - (Vector2)transform.position).normalized;

        }

        spriteRenderer.flipX = direction.x > 0;

    }
}
