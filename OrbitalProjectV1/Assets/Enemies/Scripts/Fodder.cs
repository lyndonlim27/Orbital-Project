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
    private float atkRange = 1.5f;


    private void Awake()
    {
        this.state = State.Roaming;
        this.castTrigger = Random.Range(1f, 600f);
        this.castCounter = 0;
        this.magicRange = 5f;
        
    }
    override

        public void Attack()
    {
        if (spriteRenderer.flipX == true)
        {
            customWeapon.AttackRight();
        }
        else
        {
            customWeapon.AttackLeft();
        }


    }

    public void StopAttack()
    {
        customWeapon.StopAttack();
    }

    private float Rand()
    {
        return Random.Range(1f, 1800f);

    }

    private void Update()
    {
        float steps = speed * Time.deltaTime;
        float dist;
        Vector2 direction;

        Debug.Log(target);

        if (target != null)
        {
            dist = Vector3.Distance(transform.position, target.position);
            
            animator.SetBool("isWalking", dist > atkRange);
            animator.SetBool("Attacking", dist <= atkRange && castCounter < castTrigger);
            direction = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
            if (castCounter >= castTrigger)
            {
                this.animator.Play("Cast");
                this.castCounter = 0;
                this.castTrigger = Rand();
                EnemySpell enemySpell = Instantiate(this.spellprefab, target.position + new Vector3(0,2f,0) , Quaternion.identity);
            }
            else
            {
                castCounter++;
            }

            if (animator.GetBool("Attacking") || animator.GetBool("Casting"))
            {
                //do nothing when attacking

            }
            else
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
            animator.SetBool("isWalking", true);
            animator.SetBool("Attacking", false);

        }

        spriteRenderer.flipX = direction.x > 0;

    }

    public void ResetCounter()
    {
        
    }
}

