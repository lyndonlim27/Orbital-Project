using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform SlimeGFX;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reached = false;
    Pathfinding.Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Pathfinding.Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }
    
    void UpdatePath() {
        if (seeker.IsDone())
            seeker.StartPath(rb.position,target.position,OnPathComplete);
    }

    void OnPathComplete(Pathfinding.Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reached = true;
            return;
        } else {
            reached = false;
        }
        //getting unit vector -> direction of vector
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

    }


}
