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
    private float speed;
    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool armed;
    protected GameObject target;
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    private Pathfinding.Seeker seeker;
    //private int currentWaypoint = 0;
    //private bool reached = false;
    private Pathfinding.Path path;
    protected State state;
    protected bool canMove;
    protected bool reached;


    public abstract void Attack(GameObject target);

    protected void Start()
    {
        startingPos = transform.position;
        seeker = GetComponent<Pathfinding.Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        reached = false;
        animator = GetComponent<Animator>();
    }

    protected Vector3 GetRoamingPosition()
    {
        return (startingPos + UtilsClass.GetRandomDir() * Random.Range(5f, 5f));
    }

    protected void FindTarget()
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) < aggroRange) {
            //player within aggro range;
            this.state = State.Chase;
        }
    }

    /// <summary>
    /// Pathfinding method using A* package
    /// </summary>
    /// <param name="pos"></param>
    protected void moveToPosition(Vector3 pos)
    {
        //seeker.StartPath(rb.position, pos, OnPathComplete);
        Vector2 direction = ((Vector2)pos - (Vector2)rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        this.rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, pos);
        animator.SetBool("isWalking", true);

        //handling of character orientation.
        //this.spriteRenderer.flipX = rb.velocity.x < 0;

        if (rb.velocity.x >= 0.01f)
        {
            this.transform.localScale = new Vector3(2.5f, 3f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            this.transform.localScale = new Vector3(-2.5f, 3f, 1f);
        }

    }

    //protected void OnPathComplete(Pathfinding.Path p)
    //{
    //    if (!p.error)
    //    {
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    protected void StopMoving()
    {
        this.reached = true;
        this.animator.SetBool("isWalking", false);
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

    /// <summary>
    /// Animation movement control, maybe i will switch to this to control animation of enemy instead of states. 
    /// </summary>
    //public void LockMovement()
    //{
    //    canMove = false;
    //}

    //public void UnlockMovement()
    //{
    //    canMove = true;
    //}
}
