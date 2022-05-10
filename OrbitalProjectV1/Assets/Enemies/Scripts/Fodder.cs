using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : Enemy 
{

    private float atkRange = 1f;
    private float nextAttackTime;
    public customWeapon customWeapon;

    private void Awake()
    {
        this.state = State.Roaming;
        
    }
    // Start is called before the first frame update
   
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

        animator.SetBool("isWalking",false);
        animator.SetBool("Attacking",true);
        
        
    }

    private void FixedUpdate()
    {
        switch (state)
        {
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
                if (Vector3.Distance(transform.position, target.transform.position) < atkRange)
                {
                    if (Time.time > nextAttackTime)
                    {
                        state = State.Attack;
                        
                    }
                }
                float stopChaseDistance = 10f;
                if (Vector3.Distance(transform.position, target.transform.position) > stopChaseDistance)
                {
                    //go back to current position;
                    state = State.Stop;
                }
                break;
            case State.Attack:
                //while attacking, do nothing;
                Attack(target); // stop moving when attacking.
                float fireRate = .03f; //can use the animation time instead;
                nextAttackTime = Time.time + fireRate;
                break;

            case State.Stop:
                moveToPosition(startingPos);
                if (Vector3.Distance(transform.position, startingPos) < nextWaypointDistance)
                {
                    state = State.Roaming;
                }

                break;
        }

    }

}
