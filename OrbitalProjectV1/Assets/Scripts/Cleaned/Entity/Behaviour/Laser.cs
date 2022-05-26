using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : RangedBehaviour
{
    public int damageValue;

    private Coroutine Lookcoroutine;
    private float speed = 1f;
    private Collider2D col;
    private Player player;


    private void Awake()
    {
        Player player = GameObject.FindObjectOfType<Player>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damageValue);
            
        }
    }

    public void RotateTowardsPlayer()
    {
        Vector3 dir = (transform.position - player.transform.position).normalized;
        dir = -dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void StartRotating(Transform target)
    {
        if (Lookcoroutine != null)
        {
            StopCoroutine(Lookcoroutine);
        }

        Lookcoroutine = StartCoroutine(LookAt(target));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * speed;

            yield return null;
        }
    }

    public void ResetParentState()
    {
        EnemyBehaviour parent = GetComponentInParent<EnemyBehaviour>();
        parent.stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
    }
    

}
