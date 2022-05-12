using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customWeapon : MonoBehaviour
{
    Vector2 rightAttackOffset;
    public CapsuleCollider2D swordCollider;
    public float damage = 3;
<<<<<<< HEAD
    private float force = 5f;
    private Rigidbody2D rb;
=======
>>>>>>> parent of 232f1837 (committing undone files)

    private void Start()
    {
        rightAttackOffset = transform.position;
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;

    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //deal damage to enemy;
            //i need player script.
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
<<<<<<< HEAD
                float dist = Vector2.Distance(player.transform.position,transform.position);
                float atkRange = transform.localScale.x + 1f;
                if (dist < atkRange)
                {
                    this.GetComponentInParent<Animator>().SetTrigger("Melee");
                }
                Vector2 direction = (player.transform.position - transform.position).normalized;
                player.rb.AddForce(direction * force, ForceMode2D.Impulse);
                player.TakeDamage(damage);
=======
                player.health -= damage;
>>>>>>> parent of 232f1837 (committing undone files)
            }
        }
    }
}
