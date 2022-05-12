using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : Enemy 
{

    private float atkRange = 1.5f;
    private float nextAttackTime;
    public customWeapon customWeapon;

    private void Awake()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        this.castTrigger = Random.Range(1f, 600f);
        this.castCounter = 0;
=======
        this.state = State.Roaming;
        
>>>>>>> parent of 232f1837 (committing undone files)
    }

=======
        this.state = State.Roaming;
        
    }
    // Start is called before the first frame update
   
<<<<<<< HEAD
>>>>>>> parent of 232f1837 (committing undone files)
=======
>>>>>>> parent of 232f1837 (committing undone files)
    override
        public void Attack(GameObject target)
    {
        Vector2 direction = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
        if (direction.x >= 0.01f)
        {
            this.transform.localScale = new Vector3(2.5f, 3f, 1f);
            customWeapon.AttackRight();
        }
        else if (direction.x <= -0.01f)
        {
            this.transform.localScale = new Vector3(-2.5f, 3f, 1f);
            customWeapon.AttackLeft();
        }

<<<<<<< HEAD
<<<<<<< HEAD

    }

    private void Update()
    {
        if (target != null && castCounter >= castTrigger)
        {
            castTrigger = Rand();
            castCounter = 0;
            animator.SetTrigger("Casting");

        }
=======
=======
>>>>>>> parent of 232f1837 (committing undone files)
        
        state = State.Chase;
        
        
<<<<<<< HEAD
>>>>>>> parent of 232f1837 (committing undone files)
=======
>>>>>>> parent of 232f1837 (committing undone files)
    }

    private void FixedUpdate()
    {
        switch (state)
        {
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
            default:
            case State.Roaming:

                moveToPosition(roamPosition);
                if (Vector3.Distance(transform.position, roamPosition) < nextWaypointDistance || rb.velocity.x == 0)
                {
                    //Reached roam pos
                    roamPosition = GetRoamingPosition();
                }
                FindTarget();
                break;
            case State.Chase:
                moveToPosition(target.transform.position);
                //for ranged Enemies have to tweak abit.
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist < atkRange)
                {
                    if (Time.time > nextAttackTime)
                    {
                        state = State.Attack;
                        Attack(target); // stop moving when attacking.
                        float fireRate = .03f; //can use the animation time instead;
                        nextAttackTime = Time.time + fireRate;
                        
                        
                    } 
                       
                }
                float stopChaseDistance = 3f;
                if (dist > stopChaseDistance)
                {
                    //go back to current position;
                    state = State.Stop;
                }
                animator.SetBool("isWalking", dist > atkRange);
                animator.SetBool("Attacking", dist < atkRange);
                break;
            case State.Attack:
                //while attacking, do nothing;
                break;

            case State.Stop:
                moveToPosition(startingPos);
                if (Vector3.Distance(transform.position, startingPos) < nextWaypointDistance)
                {
                    state = State.Roaming;
                }

                break;
        }
>>>>>>> parent of 232f1837 (committing undone files)
=======
            default:
            case State.Roaming:

                moveToPosition(roamPosition);
                if (Vector3.Distance(transform.position, roamPosition) < nextWaypointDistance || rb.velocity.x == 0)
                {
                    //Reached roam pos
                    roamPosition = GetRoamingPosition();
                }
                FindTarget();
                break;
            case State.Chase:
                moveToPosition(target.transform.position);
                //for ranged Enemies have to tweak abit.
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist < atkRange)
                {
                    if (Time.time > nextAttackTime)
                    {
                        state = State.Attack;
                        Attack(target); // stop moving when attacking.
                        float fireRate = .03f; //can use the animation time instead;
                        nextAttackTime = Time.time + fireRate;
                        
                        
                    } 
                       
                }
                float stopChaseDistance = 3f;
                if (dist > stopChaseDistance)
                {
                    //go back to current position;
                    state = State.Stop;
                }
                animator.SetBool("isWalking", dist > atkRange);
                animator.SetBool("Attacking", dist < atkRange);
                break;
            case State.Attack:
                //while attacking, do nothing;
                break;

            case State.Stop:
                moveToPosition(startingPos);
                if (Vector3.Distance(transform.position, startingPos) < nextWaypointDistance)
                {
                    state = State.Roaming;
                }

                break;
        }
>>>>>>> parent of 232f1837 (committing undone files)

    }

}
