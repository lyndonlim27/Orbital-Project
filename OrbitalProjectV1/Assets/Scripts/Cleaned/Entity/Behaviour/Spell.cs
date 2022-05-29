using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : RangedBehaviour
{
    private Collider2D col;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        spriteRenderer.sprite = rangedData.sprite;
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", rangedData.ac_name)) as RuntimeAnimatorController;
        transform.position = rangedData.type == RangedData.Type.CAST_ONTARGET ? player.transform.position : transform.position;
        _animator.SetTrigger(rangedData.trigger);

    }


    private void OnEnable()
    {
        col.enabled = true;
    }

    private void Disable()
    {

        col.enabled = false;
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
