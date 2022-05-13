using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public Animator animator;
    public Collider2D col;
    private float force = 3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
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
}
