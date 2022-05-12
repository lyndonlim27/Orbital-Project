using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customWeapon : MonoBehaviour
{
    Vector2 leftAttackOffset;
    public CapsuleCollider2D swordCollider;
    public float damage = 3;
    private float force = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        leftAttackOffset = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(leftAttackOffset.x * -1, leftAttackOffset.y);

    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = leftAttackOffset;

    }

    public void SwordAttack()
    {
        // use transform position of enemy to attack ?? hmm
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
            PlayerMove player = other.GetComponent<PlayerMove>();

            if (player != null)
            {
                Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                //player.rb.AddForce(direction * force, ForceMode2D.Impulse);
                player.TakeDamage(damage);
            }
        }
    }
}