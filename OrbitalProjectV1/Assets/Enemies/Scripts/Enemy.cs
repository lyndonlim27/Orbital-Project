using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    // Start is called before the first frame update
    private Weapon weapon;
    private Vector3 aggroRadius;
    private enum State {
        Idle,
        Chase,
        Stop,
        Attack,
        Death,
    }
    private State state;
    private int health;
    private float speed;
    private Rigidbody2D rb;
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool armed;
    private GameObject target;

    void attack(GameObject target)
    {

    }

    void start()
    {

    }

    void update()
    {

    }

    void exit()

    {

    }

    void IEnemy.attack(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    void IEnemy.start()
    {
        throw new System.NotImplementedException();
    }

    void IEnemy.update()
    {
        throw new System.NotImplementedException();
    }

    void IEnemy.exit()
    {
        throw new System.NotImplementedException();
    }
}
