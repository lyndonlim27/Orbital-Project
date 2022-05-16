using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    private Collider2D col;

    private void Awake()
    {
        this.col = GetComponent<Collider2D>();
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
                //Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                //Debug.Log(direction);
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.down * SpellStats.speed, ForceMode2D.Impulse);
                player.TakeDamage(SpellStats.damage);
            }
            

        }
    }
}
