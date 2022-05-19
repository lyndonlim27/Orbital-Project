using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Bullet
{

    private float lifeTime = 3.0f;
    private int damageval = 3;

    protected override void Start()
    {
        base.Start();
        animationname = "Play";
        Destroy(gameObject, lifeTime);
    }    


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //Setting collision to true so that it will trigger the next state
        //for bullet and thus play the explosion animation
        _animator.SetBool(animationname, true);
        _target.GetComponent<Player>().TakeDamage(damageval);
    }

}
