using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    private Transform slimeGFX;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Animator animator;

    private enum State
    {
        Roaming,
        Chasing,
        Stop,
        Attack,
        Death
    }

    private State state;
    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reached = false;
    Pathfinding.Seeker seeker;
    Rigidbody2D rb;
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public Transform SlimeGFX { get => slimeGFX; set => slimeGFX = value; }

    public float health = 1; //default


    private void Defeated()
    {
        animator.SetBool("isAlive", false);
    }

    private void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        seeker = GetComponent<Pathfinding.Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        { //or touch player obj
            reached = true;
            return;
        }
        else
        {
            reached = false;
        }
        //getting unit vector -> direction of vector
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);
        animator.SetBool("isWalking", !reached);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            SlimeGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            SlimeGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    private Vector3 getRoamingPosition()
    {
        return SlimeGFX.position + (Vector3)Random.insideUnitCircle.normalized * Random.Range(10f, 70f);
    }

    //private void findTarget()
    //{
    //    float targetRange = 50f;
    //    if (Vector3.Distance(transform.position, target.position) < targetRange)
    //    {
    //        //player within range, start chasing
    //    }
    //}
}
