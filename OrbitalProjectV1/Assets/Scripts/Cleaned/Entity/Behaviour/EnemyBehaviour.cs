using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Pathfinding;

//[RequireComponent(typeof(EnemyData), typeof(ConsumableItemData))]
public class EnemyBehaviour : ItemWithTextBehaviour 
{
    [SerializeField] private FodderHealthBar _healthBar;

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
    //public StateMachine stateMachine { get; protected set; }
    public StateMachine stateMachine;
    public StateMachine.STATE currstate;
    public MeleeComponent melee { get; protected set; }
    public RangedComponent ranged { get; protected set; }
    public Rigidbody2D rb { get; protected set; }
    public Animator animator { get; protected set; }
    public bool insideStage2;

    public Vector2 roamPos { get; protected set; }
    public float maxDist { get; protected set; }
    public Vector2 startingpos { get; protected set; }
    public Player player { get; protected set; }
    
    protected bool facingRight;
    private MonsterTextLogic tl;
    protected Transform _transform;
    protected DamageFlicker _flicker;
    public DetectionScript detectionScript;
    private Light2D light2D;
    private CapsuleCollider2D body;

    /** Pathfinding Movement
     * 
     */
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    private float nextWaypointDistance = 1f;
    private bool reachedEndOfPath;


    [Header("Cooldowns")]
    [SerializeField] protected float Dashcooldown;
    [SerializeField] protected float cooldown;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<Player>(true);
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = false;
        melee = GetComponentInChildren<MeleeComponent>(true);
        ranged = GetComponentInChildren<RangedComponent>(true);
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        seeker = GetComponent<Pathfinding.Seeker>();
        tl = GetComponentInChildren<MonsterTextLogic>(true);
        light2D = GetComponentInChildren<Light2D>(true);
        
        
    }

    public virtual void Start()
    {
       
        maxDist = 100f;
        
    }

    protected virtual void OnEnable()
    {
        health = enemyData.words;
        isDead = false;
        _transform = transform.parent;
        DisableAnimator();
        SettingUpColliders();
        EnableAnimator();
        ranged.gameObject.SetActive(enemyData.rangedtriggers.Count > 0);
        ranged.rangeds = enemyData.rangedDatas;
        melee.gameObject.SetActive(enemyData.meleetriggers.Count > 0);
        ResettingColor();
        light2D.color = spriteRenderer.color;
        light2D.pointLightOuterRadius = enemyData.scale * 2;
        insideStage2 = false;
        rb = GetComponentInParent<Rigidbody2D>();
        
        
    }

    private void EnableAnimator()
    {
        animator.enabled = true;
        animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", enemyData.animatorname)) as RuntimeAnimatorController;
        
    }

    private void DisableAnimator()
    {
        animator.enabled = false;
        animator.runtimeAnimatorController = null;
    }

    private void SettingUpColliders()
    {
        Debug.Log(enemyData.sprite);
        Debug.Log(enemyData.sprite.bounds.size);
        
        body = GetComponentInParent<CapsuleCollider2D>();
        if (body!=null)
        {
            body.size = enemyData.sprite.bounds.size * enemyData.scale;
            body.offset = new Vector2(0, 0);
            BoxCollider2D coll = melee.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(enemyData.sprite.bounds.size.x * 0.8f, enemyData.sprite.bounds.size.y * 0.8f);
            coll.offset = new Vector2(0.2f, 0);
        }
        
    }

    // later than onenable, making use of spawning window time.
    public virtual void Initialize() {
        
        
        
        animator.SetBool("isAlive", true);
        animator.SetBool("NotSpawned", true);
        //_rb.WakeUp();
        //transform.parent.position = transform.position;
        //transform.position = Vector3.zero;
        cooldown = 2.5f;
        startingpos = _transform.position;
        
       
        
    }

    public virtual void Update()
    {
        currstate = stateMachine.currState;
        if (isDead)
        {
            return;
        }
        CheckForStage2();
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
        Debug.Log("entered");
        if (animator.applyRootMotion == true)
        {
            Debug.Log("Dafuq");
            //_transform.position = transform.position;
            //transform.position = Vector3.zero;
            //if (transform.position.x > currentRoom.GetRoomAreaBounds().max.x || transform.position.y > currentRoom.GetRoomAreaBounds().max.y)
            //{
            //    var minX = Mathf.Min(transform.position.x, currentRoom.GetRoomAreaBounds().max.x - 1.5f);
            //    var minY = Mathf.Min(transform.position.y, currentRoom.GetRoomAreaBounds().max.y - 1.5f);
            //    _transform.position = new Vector2(minX, minY);
            //}

            //else if (transform.position.x < currentRoom.GetRoomAreaBounds().min.x || transform.position.y < currentRoom.GetRoomAreaBounds().min.y)
            //{
            //    var maxX = Mathf.Max(transform.position.x, currentRoom.GetRoomAreaBounds().min.x + 1.5f);
            //    var maxY = Mathf.Max(transform.position.y, currentRoom.GetRoomAreaBounds().min.y + 1.5f);
            //    _transform.position = new Vector2(maxX, maxY);
            //} else
            //{
            _transform.position = transform.position;
            //melee.GetComponent<BoxCollider2D>().bounds.
            transform.position = Vector3.zero;
            //GetComponentInParent<Rigidbody2D>().MovePosition(transform.position);
            
        }
        
    }

    public virtual void resetCooldown()
    {
        // use enemydata.rangedcooldown, nexttime then i add numbers to the enemy datas. for now just use 10.
        this.cooldown = enemyData.rangedcooldown;
        inAnimation = false;
        //resetPosition();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        Debug.Log(stateMachine.currState);
    }

    public void getNewRoamPosition()
    {
        //roamPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        roamPos = this.currentRoom.GetRandomPoint();


    }

    public void moveToRoam()
    {
        if (!reachedEndOfPath)
        {

        }
        AStarMove(roamPos, enemyData.moveSpeed);
        flipFace(roamPos);
    }

    private void AStarMove(Vector2 destination, float speed)
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            //seeker.StartPath(_transform.position, destination, OnPathComplete);
            seeker.StartPath(transform.position, destination, OnPathComplete);
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
            distanceToWaypoint = Vector3.Distance(_transform.position, path.vectorPath[currentWaypoint]);
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
        Vector3 dir = (path.vectorPath[currentWaypoint] - _transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;
        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
        //_transform.position += velocity * Time.deltaTime;
        rb.velocity += (Vector2) velocity * Time.deltaTime;
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
        flipFace(startingpos);
    }

    public bool isReached()
    {
        //return Vector2.Distance(roamPos, _transform.position) <= 0.1f;
        return Vector2.Distance(roamPos, transform.position) <= 0.1f;
    }

    public void flipFace(Vector2 target)
    {
        //Vector2 dir = (target - (Vector2)_transform.position).normalized;
        //if (dir.x < 0 && !facingRight)
        //{
        //    Flip();
        //}
        //else if (dir.x > 0 && facingRight)
        //{
        //    Flip();
        //}

        Vector2 dir = (target - (Vector2)transform.position).normalized;
        if (dir.x < 0 && !facingRight)
        {
            Flip();
        }
        else if (dir.x > 0 && facingRight)
        {
            Flip();
        }

    }

    private void Flip()
    {
        //Vector3 currentFace = _transform.localScale;
        //currentFace.x *= -1;
        //_transform.localScale = currentFace;
        //Vector3 _tf = tl.transform.localScale;
        //_tf.x *= -1;
        //tl.transform.localScale = _tf;
        //facingRight = !facingRight;

        //if (_healthBar != null)
        //{
        //    Vector3 scaleHealthBar = _healthBar.transform.localScale;
        //    scaleHealthBar.x *= -1;
        //    _healthBar.transform.localScale = scaleHealthBar;
        //}

        Vector3 currentFace = transform.localScale;
        currentFace.x *= -1;
        transform.localScale = currentFace;
        Vector3 _tf = tl.transform.localScale;
        _tf.x *= -1;
        tl.transform.localScale = _tf;
        facingRight = !facingRight;

        if (_healthBar != null)
        {
            Vector3 scaleHealthBar = _healthBar.transform.localScale;
            scaleHealthBar.x *= -1;
            _healthBar.transform.localScale = scaleHealthBar;
        }

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
        //_transform.position = currentRoom.GetRandomPoint();
        transform.localPosition = Vector3.zero;
        Dashcooldown = 30f;
        inAnimation = false;
        EnableAttackComps();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        this.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void CheckForStage2()
    {
        if (!insideStage2 && enemyData.stage2 && health <= (0.5f * enemyData.words) &&!inAnimation)
        {
            animator.SetTrigger("stage2intro");
            animator.SetBool("stage2", true);
            insideStage2 = true;
            spriteRenderer.material.color = enemyData.enragedColor;
            light2D.color = enemyData.enragedColor;
        }
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
                inAnimation = false;
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                
                break;
            case ANIMATION_CODE.CAST_START:
                flipFace(player.transform.position);
                SpellAttack();
                break;
            case ANIMATION_CODE.SHOOT_START:
                //animator.applyRootMotion = false;
                flipFace(player.transform.position);
                if (insideStage2)
                {
                    RandomShoot();
                } else
                {
                    Shoot();
                }
                resetPosition();

                break;
            case ANIMATION_CODE.CAST_END:
                //resetCooldown();
                //resetPosition();
                resetPosition();
                resetCooldown();
                //inAnimation = false;
                //stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
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

    public override void TakeDamage(int damage)
    {
        //Debug.Log(damage);
        //health--;
        //Debug.Log(health);
        _flicker.Flicker(this);
        base.TakeDamage(damage);
        animator.SetTrigger("Hurt");
    }

    private void SpawnCorpse()
    {
        //corpses wont be disabled, so no need to use objectpooling
        GameObject corpse = new GameObject("Corpse");
        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        var sprite = Resources.Load<Sprite>("Sprites/corpse");
        Debug.Log(sprite);
        corpseRenderer.sprite = sprite;
        corpse.transform.position = animator.transform.position;
        corpse.layer = 1;
        corpse.transform.SetParent(_transform);
        
    }

    //private void SpawnDrops()
    //{
    //    int rand = Random.Range(0, 5);
    //    Debug.Log(rand);
    //    for (int i = 0; i < rand; i ++)
    //    {
    //        Debug.Log("This is drop" + i);
    //        int rand2 = Random.Range(0, consumableItemDatas.Count);
    //        ConsumableItemBehaviour con = poolManager.GetObject(EntityData.TYPE.CONSUMABLE_ITEM) as ConsumableItemBehaviour;
    //        ConsumableItemData condata = consumableItemDatas[rand2];
    //        if (condata._consumableType == ConsumableItemData.CONSUMABLE.LETTER)
    //        {
    //            ConsumableItemData temp = condata;
    //            string passcode = FindObjectOfType<WordBank>().passcode;
    //            Debug.Log(passcode);
    //            int randomnum = Random.Range(0, passcode.Length);
    //            temp.letter = passcode[randomnum];            
    //            passcode = passcode.Substring(0, randomnum) + passcode.Substring(randomnum, passcode.Length - (randomnum+1));
    //            temp.sprite = temp.letters[(int) temp.letter - 81];
    //        }

    //        con.SetEntityStats(condata);
    //        con.transform.position = transform.position + new Vector3(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f));
    //        con.SetTarget(player.gameObject);
    //        con.gameObject.SetActive(true);
    //    }
    //}

    
    public override void Defeated()
    {
        Debug.Log("Dafuq?");
        animator.SetBool("isAlive", false);
        StartCoroutine(FadeOut());
       

    }

    protected override IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        SpawnCorpse();
        SpawnDrops();
        poolManager.ReleaseObject(this);
        
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.enemyData = Instantiate((EnemyData) stats);
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
        return Vector2.Distance(_transform.position, startingpos) >= maxDist;
    }

    //to prevent the object from jumping outside. technically shudnt happen when there are colliders but well.
    public bool CheckInsideRoom()
    {
        bool insidex = Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().max.x) > 2.5f && Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().min.x) > 2.5f;
        bool insidey = Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().max.y) > 2.5f&& Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().min.y) > 2.5f;
        Debug.Log(insidex);
        Debug.Log(insidey);
        return insidex && insidey;
    }


    public void Dodge()
    {
        if (!inAnimation && CheckInsideRoom())
        {
            flipFace(-player.transform.position);
            inAnimation = true;
            List<string> dodges = enemyData.defends;
            if (dodges.Count != 0)
            {
                int random = Random.Range(0, dodges.Count);
                animator.SetTrigger(dodges[random]);
                DisableAttackComps();

            }

        }
        else
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
    }

    private void DisableAttackComps()
    {
        if (melee != null)
        {
            melee.gameObject.SetActive(false);
        }

        if (ranged != null)
        {
            ranged.gameObject.SetActive(false);
        }


    }

    private void EnableAttackComps()
    {
        if (melee != null)
        {
            melee.gameObject.SetActive(enemyData.meleetriggers.Count > 0);
        }
        if (ranged != null)
        {
            ranged.gameObject.SetActive(enemyData.rangedtriggers.Count > 0);
        }

    }

    public void ResetDash()
    {
        Dashcooldown = 15f;
        inAnimation = false;
        //transform.parent.position = transform.position;
        //transform.localPosition = Vector3.zero;
        resetPosition();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        EnableAttackComps();
    }

    public bool CheckDistance()
    {
        return Vector2.Distance(_transform.position, player.transform.position) > 3f;
    }


    public void RandomShoot()
    {
        

        ranged.ShootSingleSphere();
        

    }

    //public void CastSpell()
    //{
    //    List<string> spellcasts = enemyData.rangedtriggers;
    //    int rand = Random.Range(0, spellcasts.Count);
    //    GetComponent<Animator>().Play(spellcasts[rand]);
    //}

}
