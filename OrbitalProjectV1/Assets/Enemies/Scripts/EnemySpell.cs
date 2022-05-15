using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public Animator animator;
    public Collider2D col;
    private float force = 3f;
    private int damage = 5;
    private float knockbackTime = 3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            player.TakeDamage(damage);
            //StartCoroutine(KnockBackCo(player.rb));
        }
        
    }

    public void OnEnable()
    {
        this.col.enabled = true;
    }

    public void Disable()
    {
        this.col.enabled = false;
    }

    public void OnDestroy()
    {

        Destroy(this.gameObject, .5f);
    }

    private IEnumerator KnockBackCo(Rigidbody2D rb)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockbackTime);
            rb.velocity = Vector2.zero;
        }
    }
}
