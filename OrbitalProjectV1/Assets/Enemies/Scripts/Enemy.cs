using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    protected float reachedPosDistance = 10f;
    // Start is called before the first frame update
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
    private int currentWaypoint = 0;
    private bool reached = false;
    private float nextWaypointDistance = 3f;
    private Pathfinding.Path path;
    protected State state;


    public abstract void Attack(GameObject target);

    private void Start()
    {
        startingPos = transform.position;
        roamPosition = GetRoamingPosition();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected Vector3 GetRoamingPosition()
    {
        return (startingPos + UtilsClass.GetRandomDir() * Random.Range(10f, 70f));
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
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)pos).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        this.rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    protected void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void StopMoving()
    {
        this.reached = true;
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
