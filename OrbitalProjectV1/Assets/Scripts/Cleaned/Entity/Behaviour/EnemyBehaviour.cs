using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : EntityBehaviour
{

    //animation handler;
    public enum ANIMATION_CODE
    {
        ATTACK_END,
        CAST_END,
        MELEE_TRIGGER,
        HURT_END,
        WEAP_TRIGGER,
        SHOOT_START,
        CAST_START,
    }

    //data;
    public EnemyData enemyData;
    public StateMachine stateMachine;
    //private int cooldown;
    protected DamageFlicker _flicker;
    public bool isDead;

    [Header("Entity Position")]
    public Vector2 roamPos;
    public float maxDist;

    public MeleeComponent melee { get; private set; }
    public RangedComponent ranged { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; protected set; }

    public DetectionScript detectionScript;
    public Vector2 startingpos;
    public Player player;
    public int health;
    public float cooldown;
    public bool facingRight;
    public bool stage2;
    private MonsterTextLogic tl;

    /** Pathfinding
     * 
     */
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    public float nextWaypointDistance = 1f;
    public bool reachedEndOfPath;

    public virtual void Start()
    {

        player = GameObject.FindObjectOfType<Player>(true);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", enemyData.animatorname)) as RuntimeAnimatorController;
        animator.keepAnimatorControllerStateOnDisable = false; 
        melee = GetComponentInChildren<MeleeComponent>();
        ranged = GetComponentInChildren<RangedComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tl = GetComponentInChildren<MonsterTextLogic>();
        spriteRenderer.sprite = enemyData.sprite;
        startingpos = GetComponent<Transform>().position;
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        isDead = false;
        seeker = GetComponent<Pathfinding.Seeker>();
        health = enemyData.words;
    }

    public virtual void Initialize() {
        animator.SetBool("isAlive", true);
        animator.SetBool("NotSpawned", true);
        transform.parent.position = transform.position;
        transform.position = Vector3.zero;
        cooldown = 2.5f;
    }

    public virtual void Update()
    {
        stateMachine.Update();
        tick();

    }

    void OnPathComplete(Pathfinding.Path p)
    {

        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
        }
    }

    public bool onCooldown()
    {
        if (ranged != null)
        {
            return this.cooldown > 0 || inAnimation;
        } else
        {
            return true;
        }

    }

    public void tick()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public virtual void resetPosition()
    {
        if (animator.hasRootMotion)
        {
            transform.parent.position = transform.position;
            transform.position = Vector3.zero;

        }
        
    }

    public virtual void resetCooldown()
    {
        this.cooldown = 10;
        inAnimation = false;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void getNewRoamPosition()
    {
        roamPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        //roamPos = this.currentRoom.GetRandomPoint();


    }

    public void moveToRoam()
    {
        AStarMove(roamPos, enemyData.moveSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, roamPos, steps);
        flipFace(roamPos);
    }

    private void AStarMove(Vector2 destination, float speed)
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.parent.position, destination, OnPathComplete);
        }
        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.parent.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.parent.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;
        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
        transform.parent.position += velocity * Time.deltaTime;
    }


    public void moveToTarget(Player player)
    {
        AStarMove(player.transform.position, enemyData.chaseSpeed);
        //transform.position = Vector3.MoveTowards(transform.position,player.transform.position,steps);
        flipFace(player.transform.position);
    }

    public void moveToStartPos()
    {
        AStarMove(startingpos, enemyData.moveSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, startingpos, steps);
        flipFace(startingpos);
    }

    public bool isReached()
    {
        return Vector2.Distance(roamPos, transform.parent.position) <= 0.1f;
    }

    public void flipFace(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.parent.position).normalized;
        if (dir.x < 0 && !facingRight)
        {
            Debug.Log("shud be right");
            Flip();
        }
        else if (dir.x > 0 && facingRight)
        {
            Debug.Log("shud be left");
            Flip();
        }

    }

    private void Flip()
    {
        Vector3 currentFace = transform.parent.localScale;
        currentFace.x *= -1;
        transform.parent.localScale = currentFace;
        Vector3 _tf = tl.transform.localScale;
        _tf.x *= -1;
        tl.transform.localScale = _tf;
        facingRight = !facingRight;
    }

    //public void RotateTowardsTarget(Vector3 pos)
    //{

    //    Vector3 dir = (pos - transform.parent.position).normalized;
    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
    //    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, 1f * Time.deltaTime);
    //    //Quaternion targetRotation = Quaternion.identity;
    //    //do
    //    //{
    //    //    Debug.Log("do rotation");

    //    //    Vector2 targetDirection = (pos - (Vector2) transform.parent.position).normalized;
    //    //    targetRotation = Quaternion.LookRotation(targetDirection);
    //    //    transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, targetRotation, Time.deltaTime);

    //    //    yield return null;

    //    //} while (Quaternion.Angle(transform.parent.rotation, targetRotation) > 0.01f);
    //}

    public virtual void Teleport()
    {
        this.transform.parent.position = roamPos;
        this.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void meleeAttack()
    {
        melee.Attack();
    }


    public virtual bool hasWeapon()
    {
        return false;
    }

    public void WeapAttack()
    {
        try
        {
            Weapon weapon = GetComponentInChildren<Weapon>();
            weapon.MeleeAttack();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("No weapon detected");
        }
    }

    public virtual void AnimationBreak(ANIMATION_CODE code)
    {
        switch (code)
        {
            case ANIMATION_CODE.MELEE_TRIGGER:
                //meleeAttack();
                flipFace(player.transform.position);
                break;
            case ANIMATION_CODE.ATTACK_END:
                resetPosition();
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                inAnimation = false;
                break;
            case ANIMATION_CODE.CAST_START:
                flipFace(player.transform.position);
                SpellAttack();
                break;
            case ANIMATION_CODE.SHOOT_START:
                //flipFace(player.transform.position);
                //resetPosition();
                Shoot();
                break;
            case ANIMATION_CODE.CAST_END:
                resetCooldown();
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                break;
            case ANIMATION_CODE.WEAP_TRIGGER:
                WeapAttack();
                break;
        }
    }

    private void SpellAttack()
    {
        ranged.SpellAttackSingle();
    }

    private void Shoot()
    {
        ranged.ShootSingle();
    }

    public virtual void Hurt()
    {
        _flicker.Flicker(this);
        health--;
        if (health == 0)
        {
            Defeated();
        } 

    }

    public override void Defeated()
    {
        //StartCoroutine(Wait());
        if (health == 0)
        {
            isDead = true;
            animator.SetBool("isAlive",false);
        } else
        {
            Hurt();
        }

    }

    ////default fade animation;
    //IEnumerator FadeOut()
    //{
    //    for (float f = 1f; f >= -0.05f; f -= 0.05f)
    //    {
    //        Color c = spriteRenderer.material.color;
    //        c.a = f;
    //        spriteRenderer.material.color = c;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    this.gameObject.SetActive(false);
    //}

    // use for enemies with death default states.
    private void DisableEnemy()
    {
        foreach (Transform tf in transform.GetComponentsInChildren<Transform>())
        {
            //disable colliders
            Collider2D[] cols = tf.GetComponents<Collider2D>();
            foreach (Collider2D col in cols)
            {
                col.enabled = false;
            }

            //disable scripts;
            MonoBehaviour[] scripts = tf.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }

            this.animator.enabled = false;

        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.enemyData = (EnemyData)stats;
    }

    public override EntityData GetData()
    {
        return enemyData;
    }

    public RoomManager GetCurrentRoom()
    {
        return this.currentRoom;
    }

    public bool TravelToofar()
    {
        return Vector2.Distance(transform.position, startingpos) >= maxDist;
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position,2f);
    //}
}
