using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : Enemy
{
    private float nextAttackTime;
    public customWeapon customWeapon;
    //public EnemySpell spellprefab;
    private float castTrigger;
    private float castCounter;


    private void Awake()
    {
        this.state = State.Roaming;
        this.castTrigger = Random.Range(1f, 600f);
        this.castCounter = 0;
    }
    // Start is called before the first frame update
    //public void Attack(GameObject target)
    // can remove if below fixedupdate works.
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

    private float Rand()
    {
        return Random.Range(1f, 600f);

    }


    private void FixedUpdate()
    {
        float steps = speed * Time.fixedDeltaTime;
        float dist;
        Vector2 direction;
        if (target != null)
        {
            dist = Vector3.Distance(transform.position, target.position);
            float atkRange = customWeapon.swordCollider.transform.localScale.x + 1f;
            animator.SetBool("isWalking", dist > atkRange);
            animator.SetBool("Attacking", dist < atkRange);
            direction = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;

            Debug.Log("This is castcounter: " + castCounter);
            Debug.Log("This is cast trigger: " + castTrigger);


            if (castCounter >= castTrigger)
            {
                animator.Play("Cast");
                EnemySpell enemySpell = Instantiate(this.spellprefab, target.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                castTrigger = Rand();
                castCounter = 0;
            }
            else
            {
                castCounter++;
            }

            if (animator.GetBool("Attacking") == true && dist < atkRange)
            {
                //do nothing when attacking

            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, steps);
                Attack(direction);

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
}