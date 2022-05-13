using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    
    [SerializeField] protected IWeapon weapon;
    [SerializeField] protected float magicRange;
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool armed;
    protected float nextWaypointDistance = 1f;
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    protected Transform target;
    protected State state;

    public abstract void Attack();

    private void Start()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected Vector3 GetRoamingPosition()
    {
        return (startingPos + UtilsClass.GetRandomDir() * Random.Range(10f, 10f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
        }
    }

    //1. this one not working also
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacles")
        {
            this.roamPosition = GetRoamingPosition();
        }
    }

    /// <summary>
    /// Death Animations. 
    /// </summary>
    ///

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Defeated();
        } 
    }

    private void Defeated()
    {
        animator.SetBool("isAlive", false);
    }

    private void RemoveEnemy()
    {
        Destroy(gameObject);
    }

}
