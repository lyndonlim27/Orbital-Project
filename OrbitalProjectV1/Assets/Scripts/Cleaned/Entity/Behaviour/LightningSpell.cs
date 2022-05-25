using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : RangedBehaviour
{
    private Collider2D col;

    private void Awake()
    {
        this.col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(rangedData.ac_name);
    }


    private void OnEnable()
    {
        this.col.enabled = true;
    }

    private void Disable()
    {

        this.col.enabled = false;
    }

    public void OnDestroy()
    {

        Destroy(this.gameObject, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (player.isDead())
                {
                    return;
                }

                player.GetComponent<Rigidbody2D>().AddForce(Vector2.down * rangedData.speed, ForceMode2D.Impulse);
                player.TakeDamage(rangedData.damage);
            }
            

        }
    }
}