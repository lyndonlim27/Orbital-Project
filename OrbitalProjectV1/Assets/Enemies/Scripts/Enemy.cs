using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private IWeapon weapon;
    [SerializeField] private float aggroRange;
    [SerializeField] private int health;
    [SerializeField] protected float speed;
    protected float nextWaypointDistance = 1f;
<<<<<<< HEAD
=======
    [SerializeField]
    private IWeapon weapon;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed;
<<<<<<< HEAD
>>>>>>> parent of 232f1837 (committing undone files)
=======
>>>>>>> parent of 232f1837 (committing undone files)
    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool armed;
<<<<<<< HEAD
<<<<<<< HEAD
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    protected Transform target;
=======
=======
>>>>>>> parent of 232f1837 (committing undone files)
    protected GameObject target;
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    private Pathfinding.Seeker seeker;
    //private int currentWaypoint = 0;
    //private bool reached = false;
    private Pathfinding.Path path;
>>>>>>> parent of 232f1837 (committing undone files)
    protected State state;
    protected bool canMove;
    protected bool reached;


    public abstract void Attack(GameObject target);

    protected void Start()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
<<<<<<< HEAD
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator.SetBool("isWalking", true);
=======
        target = GameObject.FindGameObjectWithTag("Player");
        reached = false;
        animator = GetComponent<Animator>();
>>>>>>> parent of 232f1837 (committing undone files)
=======
        target = GameObject.FindGameObjectWithTag("Player");
        reached = false;
        animator = GetComponent<Animator>();
>>>>>>> parent of 232f1837 (committing undone files)
    }

    protected Vector3 GetRoamingPosition()
    {
        return (startingPos + UtilsClass.GetRandomDir() * Random.Range(10f, 10f));
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> parent of 232f1837 (committing undone files)
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


>>>>>>> parent of 232f1837 (committing undone files)
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

    protected float Rand()
    {
        return Random.Range(1f, 600f);

    }
}
