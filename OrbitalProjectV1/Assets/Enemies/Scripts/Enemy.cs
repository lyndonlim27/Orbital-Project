using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    protected float nextWaypointDistance = 1f;
    [SerializeField]
    private IWeapon weapon;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private int health;
    [SerializeField]
    protected float speed;
    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool armed;
    //protected GameObject target;
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    private Pathfinding.Seeker seeker;
    //private int currentWaypoint = 0;
    //private bool reached = false;
    protected Transform target;
    private Pathfinding.Path path;
    protected State state;
    protected bool canMove;
    protected bool reached;

    public abstract void Attack(Vector2 dir);

    protected void Start()
    {
        startingPos = transform.position;
        seeker = GetComponent<Pathfinding.Seeker>();
        rb = GetComponent<Rigidbody2D>();
        reached = false;
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


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
        }
    }


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

    private void Defeated()
    {
        animator.SetBool("isAlive", false);
    }

    private void RemoveEnemy()
    {
        Destroy(gameObject);
    }

}