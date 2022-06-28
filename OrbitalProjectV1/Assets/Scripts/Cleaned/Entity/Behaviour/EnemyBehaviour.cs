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
    //public Animator animator { get; protected set; }
    public bool insideStage2;
    

    public Vector2 roamPos { get; protected set; }
    public float maxDist { get; protected set; }
    public Vector2 startingpos { get; protected set; }
    public Player player { get; protected set; }
    
    public bool facingRight { get; protected set; }
    protected MonsterTextLogic tl;
    protected Transform _transform;
    protected DamageFlicker _flicker;
    public DetectionScript detectionScript;
    private Light2D light2D;
    private CapsuleCollider2D body;
    private LineController lineController;
    private AstarPath astarPath;

    /**
     * AudioSources
     */

    //MaleAudios
    public List<AudioClip> malesTakeDamageAudio;
    public List<AudioClip> malesDoDamageAudio;

    public List<AudioClip> femalesTakeDamageAudio;
    public List<AudioClip> femalesDoDamageAudio;

    public List<AudioClip> nonhumanTakeDamageAudio;


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
    private Bounds bounds;
    private Patterns patterns;
    private int maxhealth;
    private float detectionRange;
    private DamageApplier damageApplier;
    private Vector2 velocityCheckPoint;

    [Header("Cooldowns")]
    [SerializeField] protected float Dashcooldown;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float healStagecooldown;
    [SerializeField] protected float animatorspeed;
    //[SerializeField] protected float 

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<Player>(true);
        melee = GetComponentInChildren<MeleeComponent>(true);
        ranged = GetComponentInChildren<RangedComponent>(true);
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        seeker = GetComponent<Pathfinding.Seeker>();
        tl = GetComponentInChildren<MonsterTextLogic>(true);
        light2D = GetComponentInChildren<Light2D>(true);
        patterns = Patterns.of(bounds.min, bounds.max);
        lineController = GetComponentInChildren<LineController>(true);
        damageApplier = GetComponentInChildren<DamageApplier>(true);
        spriteRenderer.sortingOrder = 3;
        astarPath = FindObjectOfType<AstarPath>(true);
        velocityCheckPoint = new Vector2(0.5f, 0.5f);




    }

    protected override void OnEnable()
    {
        health = enemyData.words;
        maxhealth = enemyData.words;
        isDead = false;
        inAnimation = false;
        DisableAnimator();
        ResetTransform();
        SettingUpColliders();
        EnableAnimator(); 
        _healthBar.SetMaxHealth(maxhealth);
        ranged.gameObject.SetActive(enemyData.rangedtriggers.Count > 0);
        if (ranged.isActiveAndEnabled)
        {
            lineController.ResetLineRenderer();
            
        }
        lineController.SetParent(this);
        ranged.rangeds = enemyData.rangedDatas;
        melee.gameObject.SetActive(enemyData.meleetriggers.Count > 0);
        ResettingColor();
        light2D.color = spriteRenderer.color;
        light2D.pointLightOuterRadius = enemyData.scale * 2;
        insideStage2 = false;
        maxDist = enemyData.maxDist;
        rb = GetComponentInParent<Rigidbody2D>();
        healStagecooldown = 0;
        if (damageApplier != null)
        {
            if (enemyData.attackAudios.Count > 0) {
                damageApplier.SettingUpAudio(enemyData.attackAudios[0]);
            }
            damageApplier.SettingUpDamage(enemyData.damageValue);
        }
        
        

    }

    public void RegenHealth(int hp)
    {
        if (health + hp > maxhealth)
        {
            health = maxhealth;

        }
        else
        {
            health = Mathf.Min(health + hp, maxhealth);
        }
        _healthBar.SetHealth(health);
    }

    private void ResetTransform()
    {
        _transform = transform.parent;
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;
        facingRight = false;
        RectTransform rect = _healthBar.GetComponent<RectTransform>();
        float ratio = enemyData.scale / 5;
        //rect.localPosition = new Vector3(rect.localPosition.x * ratio, rect.localPosition.y +ratio);
        _healthBar.transform.localScale = new Vector2(1,1);
        tl.transform.localScale = new Vector2(1f,1f);
        //tl.transform.localScale = new Vector2(ratio * 0.2f, ratio * 0.2f);
    }

    protected override void EnableAnimator()
    {
        animator.enabled = true;
        animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", enemyData.animatorname)) as RuntimeAnimatorController;

    }



    private void SettingUpColliders()
    {
        
        body = GetComponentInParent<CapsuleCollider2D>();
        if (body != null)
        {
            body.size = enemyData.sprite.bounds.size * enemyData.scale;
            body.offset = new Vector2(0, 0);
            BoxCollider2D coll = melee.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(enemyData.sprite.bounds.size.x * 0.8f, enemyData.sprite.bounds.size.y * 0.8f);
            coll.offset = new Vector2(0.2f, 0);
            ranged.GetComponent<CircleCollider2D>().radius = enemyData.sprite.bounds.size.x * 6f;
            detectionRange = enemyData.sprite.bounds.size.x * 6f;
            detectionScript.GetComponent<CircleCollider2D>().radius = detectionRange;
            
        }
        
    
        
    }

    // later than onenable, making use of spawning window time.
    public virtual void Initialize() {
        
        
        
        animator.SetBool("isAlive", true);
        animator.SetBool("NotSpawned", true);

        bounds = currentRoom.GetRoomAreaBounds();
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
        CheckForHealStage();
        CheckHealStageInteruption();
        stateMachine.Update();
        animatorspeed = this.animator.speed;
        tick();
    }

    public virtual void FixedUpdate()
    {
        if (rb != null)
        {
            if (rb.velocity != Vector2.zero)
            {
                StartCoroutine(FootStepAudio());
            }
        }
        
        
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Obstacles") || collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Door"))
    //    {
            
    //        if (currstate == StateMachine.STATE.ROAMING)
    //        {
                
    //            ABPath.Construct(transform.position, roamPos);

    //        }
    //        else if (currstate == StateMachine.STATE.CHASE)
    //        {
    //            ABPath.Construct(transform.position, player.transform.position);
                
    //        }

    //        //stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
    //    }
    //}

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
            healStagecooldown -= Time.deltaTime;
        }
    }

    public void InstantiateDamageAudio()
    {
        
        switch(enemyData.body)
        {
            case EnemyData.BODYTYPE.MEN:
                int rand = Random.Range(0, malesDoDamageAudio.Count);
                StartCoroutine(LoadAudio(rand,malesDoDamageAudio));
                break;
            case EnemyData.BODYTYPE.WOMEN:
                int rand2 = Random.Range(0, femalesDoDamageAudio.Count);
                StartCoroutine(LoadAudio(rand2, femalesDoDamageAudio));
                break;
            case EnemyData.BODYTYPE.MONSTER:
                /*havent found audios yet*/
                break;
        }

    }

    public void InstantiateHurtAudio()
    {
        switch (enemyData.body)
        {
            case EnemyData.BODYTYPE.MEN:
                int rand = Random.Range(0, malesTakeDamageAudio.Count);
                StartCoroutine(LoadAudio(rand, malesTakeDamageAudio));
                break;
            case EnemyData.BODYTYPE.WOMEN:
                int rand2 = Random.Range(0, femalesTakeDamageAudio.Count);
                StartCoroutine(LoadAudio(rand2, femalesTakeDamageAudio));
                break;
            case EnemyData.BODYTYPE.MONSTER:
                /*havent found audios yet*/
                break;
        }

    }

    public void PlayAudio()
    {
        if (enemyData.attackAudios.Count > 0)
        {
            audioSource.pitch = 1f;
            audioSource.clip = enemyData.attackAudios[0];
            audioSource.Play();
        }

    }

    private IEnumerator LoadAudio(int rand, List<AudioClip> audioClips)
    {
        audioSource.Stop();
        inAudio = true;
        float ogpitch = audioSource.pitch;
        audioSource.pitch = 1f;
        audioSource.clip = audioClips[rand];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        inAudio = false;
        audioSource.pitch = ogpitch;
        
    }

    public virtual void resetPosition()
    {
        
        if (animator.applyRootMotion == true)
        {
            
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
            if (body.IsTouchingLayers(LayerMask.GetMask("Obstacles", "enemy", "door"))) {
                getNewRoamPosition();
            }
        }
        AStarMove(roamPos, enemyData.moveSpeed);
        flipFace(roamPos);
    }

    protected override IEnumerator FootStepAudio()
    {
        if (!inAudio)
        {
            audioSource.pitch = 0.6f;
        }
        
        return base.FootStepAudio();

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

            if (rb.velocity.magnitude <= 0.5f || distanceToWaypoint <= nextWaypointDistance)
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
        if (body.IsTouchingLayers(LayerMask.GetMask("Obstacles", "enemy", "door")))
        {
            startingpos = currentRoom.GetRandomPoint();
        }
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
        FlipTextLogic();
        FlipHealthBar();
        facingRight = !facingRight;

    }

    private void FlipTextLogic()
    {
        if (tl != null)
        {
            Vector3 _tf = tl.transform.localScale;
            _tf.x *= -1;
            tl.transform.localScale = _tf;

        }
    }

    private void FlipHealthBar()
    {
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
        bool canActivate = !insideStage2 && enemyData.stage2 && health <= (0.5f * maxhealth) && !inAnimation;
        if (canActivate)
        {
            animator.SetTrigger("stage2intro");
            animator.SetBool("stage2", true);
            SummonOffensiveProps();
            insideStage2 = true;
            spriteRenderer.material.color = enemyData.enragedColor;
            light2D.color = enemyData.enragedColor;
        }
    }

    private void SummonOffensiveProps()
    {
        currentRoom.SpawnObjects(enemyData.bossdamageprops.ToArray());
        
    }

    private void SummonDefensiveProps()
    {
        currentRoom.SpawnObjects(enemyData.bossdefenceprops.ToArray());
    }

    private void SummonFodders()
    {
        currentRoom.SpawnObjects(enemyData.bossSummonprops.ToArray());
    }

    private void CheckForHealStage()
    {
        bool activatable = enemyData.healStage && health <= 0.5f * maxhealth && !inAnimation && currstate != StateMachine.STATE.RECOVERY && healStagecooldown <= 0;
        if (activatable)
        {
            animator.SetTrigger("Heal");
            inAnimation = true;
            LockMovement();
            SummonDefensiveProps();
            SummonFodders();
            DisableAttackComps();
            stateMachine.ChangeState(StateMachine.STATE.RECOVERY, null);
        }
    }

    private void CheckHealStageInteruption()
    {
        if (currstate == StateMachine.STATE.RECOVERY)
        {
            bool interupted = currentRoom.conditionSize == 0 || health == maxhealth;
            if (interupted)
            {
                UnlockMovement();
                EnableAttackComps();
                currentRoom.DestroyAllFodders();
                currentRoom.DestroyBossProps();
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                animator.ResetTrigger("Heal");
                inAnimation = false;
                resetHealingCooldown();
            }
        }
    }

    private void resetHealingCooldown()
    {
        healStagecooldown = 40f;
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

                UnlockMovement();
                resetPosition();
                inAnimation = false;
                InstantiateDamageAudio();
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                
                break;
            case ANIMATION_CODE.CAST_START:
                flipFace(player.transform.position);
                SpellAttack();
                InstantiateDamageAudio();
                break;
            case ANIMATION_CODE.SHOOT_START:
                //animator.applyRootMotion = false;
                flipFace(player.transform.position);
                //if (insideStage2)
                //{
                //    RandomShoot();
                //} else
                //{
                //    Shoot();
                //}
                Shoot();
                InstantiateDamageAudio();
                resetPosition();

                break;
            case ANIMATION_CODE.CAST_END:
                //resetCooldown();
                //resetPosition();
                UnlockMovement();
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

    public void LockMovement()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
    }

    public void UnlockMovement()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
    }

    private void SpellAttack()
    {
        if (insideStage2)
        {
            ranged.SpellAttackMultiple();
        } else
        {
            ranged.SpellAttackSingle();
        }
        
    }

    private void Shoot()
    {
        if (insideStage2)
        {


            ranged.ShootSingleSphere();


            //EnemyData.PATTERN pATTERN = (EnemyData.PATTERN)Random.Range(0, (int)EnemyData.PATTERN.COUNT);
            //List<Vector2> points = patterns.RandomPattern(pATTERN);
            //ranged.ShootRandomPattern(points);
        } else
        {
            ranged.ShootSingle();
        }
        
    }


    public override void TakeDamage(int damage)
    {
        //Debug.Log(damage);
        //health--;
        //Debug.Log(health);
        _flicker.Flicker(this);
        base.TakeDamage(damage);
        InstantiateHurtAudio();
        _healthBar.SetHealth(health);
        animator.SetTrigger("Hurt");
    }

    private void SpawnCorpse()
    {
        //corpses wont be disabled, so no need to use objectpooling
        GameObject corpse = new GameObject("Corpse");
        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        var sprite = Resources.Load<Sprite>("Sprites/corpse");
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
        
        animator.SetBool("isAlive", false);
        body.enabled = false;
        isDead = true;
        StartCoroutine(FadeOut());
       

    }

    public override IEnumerator FadeOut()
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
        //bool insidex = Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().max.x) > 2.5f && Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().min.x) > 2.5f;
        //bool insidey = Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().max.y) > 2.5f&& Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().min.y) > 2.5f;
        //Debug.Log("This is face" + facingRight);
        if (transform.localScale.x > 0) // facing left, 
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + 12f, transform.position.y), LayerMask.GetMask("Obstacles"));
            return !hit;
        } else
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x - 12f, transform.position.y), LayerMask.GetMask("Obstacles"));
            return !hit;
        }
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


    

    public override void Freeze()
    {
        
        if (rb != null)
        {
            LockMovement();
        }
        
        animator.speed = 0;
    }

    public override void UnFreeze()
    {
   
        if (rb !=  null)
        {
            UnlockMovement();
            
        }

        animator.speed = 1;

    }

    //public void CastSpell()
    //{
    //    List<string> spellcasts = enemyData.rangedtriggers;
    //    int rand = Random.Range(0, spellcasts.Count);
    //    GetComponent<Animator>().Play(spellcasts[rand]);
    //}

}
