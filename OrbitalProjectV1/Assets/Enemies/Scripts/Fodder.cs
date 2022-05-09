using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : Enemy 
{

    private float atkRange = 5f;
    private float nextAttackTime;

    private void Awake()
    {
        this.state = State.Roaming;
    }
    // Start is called before the first frame update

    override
        public void Attack(GameObject target)
    {
        
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Roaming:

                moveToPosition(roamPosition);
                if (Vector3.Distance(transform.position, roamPosition) < reachedPosDistance)
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
                        StopMoving();
                        Attack(target); // stop moving when attacking.
                        state = State.Attack;
                        float fireRate = .03f; //can use the animation time instead;
                        nextAttackTime = Time.time + fireRate;
                    }
                }
                float stopChaseDistance = 80f;
                if (Vector3.Distance(transform.position, target.transform.position) > stopChaseDistance)
                {
                    //go back to current position;
                    state = State.Stop;
                }
                break;
            case State.Attack:
                //while attacking, do nothing;
                break;

            case State.Stop:
                moveToPosition(startingPos);

                reachedPosDistance = 10f;
                if (Vector3.Distance(transform.position, startingPos) < reachedPosDistance)
                {
                    state = State.Roaming;
                }

                break;
        }

    }

}
