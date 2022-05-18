using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell
{

    SpriteRenderer spriteRenderer;
    Vector3 _target;
   
    // Update is called once per frame
    void Update()
    {
        transform.position += _target * SpellStats.speed * Time.deltaTime;
        transform.rotation  = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, _target, SpellStats.speed * Time.deltaTime, 0.0f));
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Explosion");
        GameObject go = collision.gameObject;
        if (go != null)
        {
            Player player = go.GetComponent<Player>();
            if (player != null)
            {
                Vector2 forcedir = spriteRenderer.flipX == true ? Vector2.left : Vector2.right;
                player.GetComponent<Rigidbody2D>().AddForce(forcedir * SpellStats.speed, ForceMode2D.Impulse);
                Destroy(this.gameObject);
            }

        }
    }
}
