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

    [Header("Roaming positions")]
    [SerializeField] Vector2 roamMinArea = new Vector2(-18f,-8f);
    [SerializeField] Vector2 roamMaxArea = new Vector2(1,6);



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

    private void Update()
    {
        float steps = speed * Time.deltaTime;
        float dist;
        Vector2 direction;

        Debug.Log(target);

        if (target != null)
        {
            dist = Vector3.Distance(transform.position, target.position);
            direction = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
            if (castCounter >= castTrigger)
            {

                animator.SetTrigger("Cast");
                EnemySpell enemySpell = Instantiate(this.spellprefab, target.position, Quaternion.identity);
                StartCoroutine(Wait());
                this.castCounter = 0;
                this.castTrigger = Rand();

            }
            else if (dist < atkRange)
            {
                castCounter++;
                animator.SetTrigger("Attack");
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

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

    }
}

