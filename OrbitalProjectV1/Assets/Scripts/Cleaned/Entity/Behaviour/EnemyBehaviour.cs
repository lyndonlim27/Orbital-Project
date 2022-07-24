using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Pathfinding;

//[RequireComponent(typeof(EnemyData), typeof(ConsumableItemData))]
public class EnemyBehaviour : ItemWithTextBehaviour
{
    #region Variables
    #region StateMachine Data
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

    public EnemyData enemyData;
    public StateMachine stateMachine;
    public StateMachine.STATE currstate;
    public StateMachine.STATE prevstate;
    public MeleeComponent melee { get; protected set; }
    public RangedComponent ranged { get; protected set; }
    public Rigidbody2D rb { get; protected set; }
    public bool insideStage2;
    [Header("Cooldowns")]
    [SerializeField] protected float Dashcooldown;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float healStagecooldown;
    [SerializeField] protected float animatorspeed;
    #endregion

    #region EnemyComponents
    [SerializeField] private FodderHealthBar _healthBar;
    protected MonsterTextLogic tl;
    protected Transform _transform;
    protected DamageFlicker _flicker;
    public DetectionScript detectionScript;
    private CapsuleCollider2D body;
    private LineController lineController;
    private AstarPath astarPath;
    private DamageApplier damageApplier;
    #endregion

    #region AudioClips
    /**
     * AudioSources
     */

    //MaleAudios
    public List<AudioClip> malesTakeDamageAudio;
    public List<AudioClip> malesDoDamageAudio;

    public List<AudioClip> femalesTakeDamageAudio;
    public List<AudioClip> femalesDoDamageAudio;

    public List<AudioClip> nonhumanTakeDamageAudio;

    #endregion


    #region PathfindingMovement
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
    private Vector2 velocityCheckPoint;
    private const float jumpHeight = 8f;
    private float originalmass;
    public Vector2 roamPos { get; protected set; }
    public float maxDist { get; protected set; }
    public Vector2 startingpos { get; protected set; }
    public bool facingRight { get; protected set; }
    #endregion
    #endregion

    #region Monobehaviour
    /** The first instance the gameobject is being activated.
     *  Retrieves all relevant data.
     */
    protected override void Awake()
    {
        base.Awake();
        melee = GetComponentInChildren<MeleeComponent>(true);
        ranged = GetComponentInChildren<RangedComponent>(true);
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

    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    protected override void OnEnable()
    {
        base.OnEnable();
        ResettingLocalBools();
        SetRbMass();
        DisableAnimator();
        ResetTransform();
        SettingUpColliders();
        EnableAnimator();
        ResettingEnemyDatas();
        ResetHp();
        ResettingLineRenderer();
        ResettingColor();
        ResettingLights();
        ResettingDamageValue();
        DestroyAllParticles();
    }

    public void StartDialogue()
    {
        if (enemyData.story != null)
        {
           // FindObjectOfType<DialogueManager>().EnterDialogue(this);
        }
    }

    /**
     * Initializing enemy animator and cooldown and startingpos after spawn animation.
     */
    public virtual void Initialize()
    {
        animator.SetBool("isAlive", true);
        animator.SetBool("NotSpawned", true);
        bounds = currentRoom.GetSpawnAreaBound();
        cooldown = 2.5f;
        startingpos = _transform.position;

    }

    private void Start()
    {
        player = Player.instance;
        _flicker = DamageFlicker.instance;
        
    }

    /**
     * Check for change of states every frame.
     */
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
        Tick();
        DashToEnemy();
    }

    private void DashToEnemy()
    {
        if (Dashcooldown < 0 && !inAnimation && currentRoom.GetSpawnAreaBound().Contains(player.transform.position))
        {
            inAnimation = true;
            _transform.position = player.transform.position + Random.insideUnitSphere;
            var selecteddash = enemyData.defends[Random.Range(0, enemyData.defends.Count)];
            animator.SetTrigger(selecteddash);
            if (enemyData.rangedDatas != null)
            {
                ranged.gameObject.SetActive(true);
                ShootAllAround(selecteddash);
            } else
            {
                ResetDash();
            }
            
        }
    }

    public void ShootAllAround(string trigger)
    {
        if (trigger == "Dash")
        {
            ranged.ShootSingleSphereDash1();
        } else if (insideStage2 && trigger == "Dash")
        {
            ranged.ShootSingleSphereDash2();
        } else
        {
            ResetDash();
            animator.SetTrigger(enemyData.meleetriggers[Random.Range(0, enemyData.meleetriggers.Count)]);
        }
        
    }

    /**
     * Check for change in entity movement every frame.
     */
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
    #endregion
    
    #region Internal Methods
    #region GarbageCollector
    private void ResettingLights()
    {
        light2D.color = spriteRenderer.color;
        light2D.pointLightOuterRadius = enemyData.scale * 2;
    }

    private void ResettingEnemyDatas()
    {
        ranged.gameObject.SetActive(enemyData.rangedtriggers.Count > 0);
        ranged.rangeds = enemyData.rangedDatas;
        melee.gameObject.SetActive(enemyData.meleetriggers.Count > 0);
        maxDist = enemyData.maxDist;
        health = enemyData.words;
        maxhealth = enemyData.words;
        healStagecooldown = 0;
        Dashcooldown = enemyData.defends.Count > 0 ? 10f : Mathf.Infinity;
    }

    private void ResetHp()
    {
        _healthBar.SetMaxHealth(maxhealth);
    }

    private void ResettingLocalBools()
    {
        Debuffed = false;
        isDead = false;
        inAnimation = false;
        insideStage2 = false;
    }

    /**
* Resets lineRenderer component.
*/
    private void ResettingLineRenderer()
    {
        if (ranged.isActiveAndEnabled)
        {
            lineController.ResetLineRenderer();

        }
        lineController.SetParent(this);
    }

    /**
     * Resets damageValue component.
     */
    private void ResettingDamageValue()
    {
        if (damageApplier != null)
        {
            damageApplier.SettingUpDamage(enemyData.damageValue);
        }
    }

    /**
     * SetRb
     */
    private void SetRbMass()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            originalmass = rb.mass;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    /**
     * Destroy all particles before enabling.
     */
    private void DestroyAllParticles()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            Destroy(particleSystem.gameObject);
        }
    }

    /**
     * Regen health.
     */
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

    /**
     * Resetting transforms to current entity size.
     */
    private void ResetTransform()
    {
        _transform = transform.parent;
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;
        facingRight = false;
        RectTransform rect = _healthBar.GetComponent<RectTransform>();
        float ratio = enemyData.scale / 5;
        //rect.localPosition = new Vector3(rect.localPosition.x * ratio, rect.localPosition.y +ratio);
        _healthBar.transform.localScale = new Vector2(1, 1);
        tl.transform.localScale = new Vector2(1f, 1f);
        //tl.transform.localScale = new Vector2(ratio * 0.2f, ratio * 0.2f);
    }

    /**
     * Enabling animator.
     */
    protected override void EnableAnimator()
    {
        animator.enabled = true;
        animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", enemyData.animatorname)) as RuntimeAnimatorController;

    }

    /**
     * Setting Up of Colliders.
     */
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
    #endregion

    #region EnemyAudios
    /**
     * Instantiate damage audio.
     */
    public void InstantiateDamageAudio()
    {

        switch (enemyData.body)
        {
            case EnemyData.BODYTYPE.MEN:
                int rand = Random.Range(0, malesDoDamageAudio.Count);
                StartCoroutine(LoadAudio(rand, malesDoDamageAudio));
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

    /**
     * Instantiate hurt audio.
     */
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

    /**
     * Play audio.
     */
    public void PlayAudio()
    {
        if (enemyData.attackAudios.Count > 0)
        {
            audioSource.pitch = 1f;
            audioSource.clip = enemyData.attackAudios[0];
            audioSource.Play();
        }

    }

    /**
     * Load Audio.
     */
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

    /**
     * FootStep Audio.
     */
    protected override IEnumerator FootStepAudio()
    {
        if (!inAudio)
        {
            audioSource.pitch = 0.6f;
        }

        return base.FootStepAudio();

    }

    #endregion

    #region AStarPathFinding

    /**
     * AStarPathfinding on complete.
     */
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
    /**
     * AStar Pathing.
     */
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
        rb.velocity += (Vector2)velocity * Time.deltaTime;
    }


    /**
     * Move To Target Position.
     */
    public void MoveToTarget(Player player)
    {
        if (currentRoom.astarPath == null)
        {
            return;
        }
        AStarMove(player.transform.position, enemyData.chaseSpeed);
        //transform.position = Vector3.MoveTowards(transform.position,player.transform.position,steps);
        FlipFace(player.transform.position);


    }

    /**
     * Move To Roam Positon.
     */
    public void moveToRoam()
    {
        if (currentRoom.astarPath == null)
        {
            return;
        }
        if (!reachedEndOfPath)
        {
            if (body.IsTouchingLayers(LayerMask.GetMask("Obstacles", "enemy", "door","PassableDeco")))
            {
                getNewRoamPosition();
            }
        }
        AStarMove(roamPos, enemyData.moveSpeed);
        FlipFace(roamPos);
    }

    /**
     * Move To Starting Position.
     */
    public void MoveToStartPos()
    {
        if (currentRoom.astarPath == null)
        {
            return;
        }
        if (body.IsTouchingLayers(LayerMask.GetMask("Obstacles", "enemy", "Doors", "Traps","PassableDeco")))
        {
            startingpos = currentRoom.GetRandomObjectPoint();
        }
        AStarMove(startingpos, enemyData.moveSpeed);
        FlipFace(startingpos);
    }

    /**
     * Get new Roam Positon.
     */
    public void getNewRoamPosition()
    {
        //roamPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        roamPos = this.currentRoom.GetRandomObjectPoint();


    }
    #endregion

    #region Movement Checkers
    /**
     * Check if reached.
     */
    public bool IsReached()
    {
        //return Vector2.Distance(roamPos, _transform.position) <= 0.1f;
        return Vector2.Distance(roamPos, transform.position) <= 0.1f;
    }

    /**
    * Check if enemy travelled too far from original position.
    */
    public bool TravelToofar()
    {
        return Vector2.Distance(_transform.position, startingpos) >= maxDist;
    }

    /**
     * Check if enemy still inside room.
     * To prevent the object from jumping outside. technically shudnt happen when there are colliders but well.
     */
    public bool CheckInsideRoom()
    {
        //bool insidex = Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().max.x) > 2.5f && Mathf.Abs(_transform.position.x - currentRoom.GetRoomAreaBounds().min.x) > 2.5f;
        //bool insidey = Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().max.y) > 2.5f&& Mathf.Abs(_transform.position.y - currentRoom.GetRoomAreaBounds().min.y) > 2.5f;
        //Debug.Log("This is face" + facingRight);
        if (transform.localScale.x > 0) // facing left, 
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + enemyData.dashoffset, transform.position.y), LayerMask.GetMask("Obstacles"));
            return !hit;
        }
        else
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x - enemyData.dashoffset, transform.position.y), LayerMask.GetMask("Obstacles"));
            return !hit;
        }
    }

    #endregion

    #region FlippingGameObject
    /**
     * Flip transform to face target.
     */
    public void FlipFace(Vector2 target)
    {

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

    /**
     * Flipping of transform.
     */
    private void Flip()
    {

        Vector3 currentFace = transform.localScale;
        currentFace.x *= -1;
        transform.localScale = currentFace;
        FlipTextLogic();
        FlipHealthBar();
        facingRight = !facingRight;

    }

    /**
     * Flipping of textUI transform.
     */
    private void FlipTextLogic()
    {
        if (tl != null)
        {
            Vector3 _tf = tl.transform.localScale;
            _tf.x *= -1;
            tl.transform.localScale = _tf;

        }
    }

    /**
     * Flipping of healthbar transform.
     */
    private void FlipHealthBar()
    {
        if (_healthBar != null)
        {
            Vector3 scaleHealthBar = _healthBar.transform.localScale;
            scaleHealthBar.x *= -1;
            _healthBar.transform.localScale = scaleHealthBar;
        }
    }

    #endregion

    #region Teleport Behaviour
    /**
     * Teleport tranform to roam position.
     */
    public virtual void Teleport()
    {
        //_transform.position = currentRoom.GetRandomPoint();
        transform.localPosition = Vector3.zero;
        Dashcooldown = 10f;
        inAnimation = false;
        EnableAttackComps();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }
    #endregion

    #region BossBehaviour

    #region SummoningBehaviour
    /**
     * Summoning of Offensive Props.
     */
    private void SummonOffensiveProps()
    {
        currentRoom.SpawnObjects(enemyData.bossdamageprops.ToArray());

    }

    /**
     * Summoning of Defensive Props.
     */
    private void SummonDefensiveProps()
    {
        currentRoom.SpawnObjects(enemyData.bossdefenceprops.ToArray());
    }

    /**
     * Summoning of Fodders.
     */
    private void SummonFodders()
    {
        currentRoom.SpawnObjects(enemyData.bossSummonprops.ToArray());
    }
    #endregion

    #region BossCheckers
    /**
     * Checking for Heal Stages.
     */
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

    /**
     * Checking for Heal Interuptions.
     */
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
                ResetHealingCooldown();
            }
        }
    }

    /**
     * Check for Stage2.
     */
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
    #endregion

    /**
     * Resetting healing cooldown.
     */
    private void ResetHealingCooldown()
    {
        healStagecooldown = 40f;
    }
    #endregion

    #region EnemyBehaviour
    /**
     * MeleeAttacks.
     */
    public void MeleeAttack()
    {
        melee.Attack();
    }

    /**
     * Check if enemy holding on weapon.
     */
    public virtual bool HasWeapon()
    {
        return false;
    }

    /**
     * Initializing weapon attacks.
     */
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

    /**
     * Check if skill on cooldown.
     */
    public bool onCooldown()
    {
        if (ranged != null)
        {
            return this.cooldown > 0 || inAnimation;
        }
        else
        {
            return true;
        }

    }

    /**
     * Reset Cooldown.
     */
    public virtual void resetCooldown()
    {
        // use enemydata.rangedcooldown, nexttime then i add numbers to the enemy datas. for now just use 10.
        this.cooldown = enemyData.rangedcooldown;
        //inAnimation = false;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

    }

    /**
     * Decrement cooldown timer.
     */
    public void Tick()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            
        }

        if (healStagecooldown > 0)
        {
            healStagecooldown -= Time.deltaTime;
        }

        if (Dashcooldown > 0)
        {
            Dashcooldown -= Time.deltaTime;
        }
    }

    /**
     * lazy to remove this animation event from all animations so im keeping it here.
     */
    public void resetPosition()
    {

    }

    #endregion

    #region AnimationBreaks
    /**
     * Animation breaks to next state.
     */
    public virtual void AnimationBreak(ANIMATION_CODE code)
    {
        switch (code)
        {
            case ANIMATION_CODE.MELEE_TRIGGER:
                FlipFace(player.transform.position);
                break;
            case ANIMATION_CODE.ATTACK_END:
                UnlockMovement();
                inAnimation = false;
                InstantiateDamageAudio();
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

                break;
            case ANIMATION_CODE.CAST_START:
                FlipFace(player.transform.position);
                SpellAttack();
                InstantiateDamageAudio();
                break;
            case ANIMATION_CODE.SHOOT_START:
                FlipFace(player.transform.position);
                Shoot();
                InstantiateDamageAudio();
                break;
            case ANIMATION_CODE.CAST_END:
                UnlockMovement();
                resetCooldown();
                inAnimation = false;
                break;
            case ANIMATION_CODE.WEAP_TRIGGER:
                WeapAttack();
                break;
            case ANIMATION_CODE.HURT_END:
                UnlockMovement();
                inAnimation = false;
                stateMachine.ChangeState(prevstate, null);
                break;

        }
    }

    #endregion

    #region EnemyHelpers
    /**
     * Lock Enemy Movement.
     */
    public void LockMovement()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }

    /**
     * Unlock Enemy Movement.
     */
    public void UnlockMovement()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }

    /**
     * Initiailize SpellAttack.
     */
    private void SpellAttack()
    {
        if (insideStage2)
        {
            ranged.SpellAttackMultiple();
        }
        else
        {
            ranged.SpellAttackSingle();
        }

    }

    public void SpellAttackSingle()
    {
        ranged.SpellAttackSelf();
    }

    /**
     * Initiailize Shooting Attack.
     */
    private void Shoot()
    {
        if (insideStage2)
        {
            ranged.ShootSingleSphere();
            //EnemyData.PATTERN pATTERN = (EnemyData.PATTERN)Random.Range(0, (int)EnemyData.PATTERN.COUNT);
            //List<Vector2> points = patterns.RandomPattern(pATTERN);
            //ranged.ShootRandomPattern(points);
        }
        else
        {
            ranged.ShootSingle();
        }

    }

    /**
     * Enemy Take Damage.
     */
    public override void TakeDamage(int damage)
    {
        _flicker.Flicker(this);
        base.TakeDamage(damage);
        InstantiateHurtAudio();
        Debug.Log("Health is: " + health);
        _healthBar.SetHealth(health);
        animator.SetTrigger("Hurt");
        prevstate = currstate;
    }

    /*
     * Spawn Corpse on death.
     */
    private void SpawnCorpse()
    {
        GameObject corpse = new GameObject("Corpse");
        SpriteRenderer corpseRenderer = corpse.AddComponent<SpriteRenderer>();
        var sprite = Resources.Load<Sprite>("Sprites/corpse");
        corpseRenderer.sprite = sprite;
        corpse.transform.position = animator.transform.position;
        corpse.layer = 1;
        corpse.transform.SetParent(_transform);

    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {

        animator.SetBool("isAlive", false);
        body.enabled = false;
        isDead = true;
        StartCoroutine(FadeOut());


    }

    /**
     * FadeOut Animation.
     */
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



    /**
     * Enemy dodge motion.
     */
    public void Dodge()
    {
        if (!inAnimation && CheckInsideRoom())
        {
            FlipFace(-player.transform.position);
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

    /**
     * Disabling of attack components.
     */
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

    /**
     * Enabling of attack components.
     */
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

    /**
     * Reseting Dash.
     */
    public void ResetDash()
    {
        Dashcooldown = 10f;
        inAnimation = false;
        //transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        EnableAttackComps();
    }

    /**
     * Checking distance between enemy and player.
     */
    public bool CheckDistance()
    {
        return Vector2.Distance(_transform.position, player.transform.position) > 3f;
    }

    /**
     * Freezing enemy movement.
     */
    public override void Freeze()
    {

        if (rb != null)
        {
            LockMovement();
        }

        animator.speed = 0;
    }

    /**
     * Unfreezing enemy movement.
     */
    public override void UnFreeze()
    {

        if (rb != null)
        {
            UnlockMovement();

        }

        animator.speed = 1;

    }

    public void DashForward()
    {
        UnlockMovement();
        if (facingRight)
        {
            rb.AddForce(-transform.right * 1000f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * 1000f, ForceMode2D.Impulse);
        }

    }

    public void DashForwardBy(float force)
    {
        UnlockMovement();
        if (facingRight)
        {
            rb.AddForce(-transform.right * force, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * force, ForceMode2D.Impulse);
        }
    }

    public void DashTowardsEnemy()
    {
        UnlockMovement();
        FlipFace(player.transform.position);
        Vector2 vectorToPlayer = (player.transform.position - transform.position).normalized;
        rb.AddForce(vectorToPlayer * 1000f, ForceMode2D.Impulse);

        

    }

    public void MoveBy(string movePos)
    {
        if (movePos == "")
        {
            Debug.LogError("No input position given");
        }
        else
        {
            UnlockMovement();
            string[] mPos = movePos.Split(":");
            float x = float.Parse(mPos[0]);
            float y = float.Parse(mPos[1]);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + x, transform.position.y + y), LayerMask.GetMask("Obstacles"));

            if (facingRight)
            {

                rb.MovePosition(hit ? hit.point : new Vector2(_transform.position.x - transform.right.x * x, _transform.position.y + y));
            }
            else
            {
                rb.MovePosition(hit ? hit.point : new Vector2(_transform.position.x + transform.right.x * x, _transform.position.y + y));
            }
        }


    }

    public void TeleportToTarget()
    {
        if (player.transform == null)
        {
            return;
        }

        if (facingRight)
        {
            rb.MovePosition(player.transform.position + transform.right);
        }
        else
        {
            rb.MovePosition(player.transform.position - transform.right);
        }

    }

    #endregion

    #region Data Methods
    /**
     * Setting Enemy Stats.
     */
    public override void SetEntityStats(EntityData stats)
    {
        this.enemyData = Instantiate((EnemyData)stats);
    }

    /**
     * Retrieving Enemy Stats.
     */
    public override EntityData GetData()
    {
        return enemyData;
    }

    /**
     * Getting Current Room.
     */
    public RoomManager GetCurrentRoom()
    {
        return this.currentRoom;
    }
    #endregion
    #endregion

    #region Client-Access Methods
    #endregion
}

#region UnusedCodes
//public void LandForward()
//{
//    UnlockMovement();
//    if (facingRight)
//    {
//        rb.MovePosition(new Vector2(_transform.position.x -transform.right.x * jumpHeight, _transform.position.y - jumpHeight));
//    }
//    else
//    {
//        rb.MovePosition(new Vector2(_transform.position.x + transform.right.x * jumpHeight, _transform.position.y - jumpHeight));
//    }
//}

//public void LandForwardImmediate()
//{
//    UnlockMovement();
//    StartCoroutine(ChangeMassForAframe());

//}


//public IEnumerator ChangeMassForAframe()
//{
//    rb.mass = 50f;
//    yield return null;
//    LandForward();
//    rb.mass = originalmass;
//}

//public void JumpUp()
//{
//    //rb.AddForce(new Vector2(0, jumpHeight) * 1000f, ForceMode2D.Impulse);
//    rb.MovePosition(new Vector2(_transform.position.x, _transform.position.y + jumpHeight));
//}


//public class MovePosition : AnimationEvent
//{
//    float x;
//    float y;

//    public MovePosition(float _x, float _y)
//    {
//        x = _x;
//        y = _y;
//    }
//}

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

#endregion
