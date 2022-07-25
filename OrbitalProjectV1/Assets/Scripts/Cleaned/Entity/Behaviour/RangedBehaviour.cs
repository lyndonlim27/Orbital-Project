using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a general class for ranged behaviours.
/// It handles the different behaviours for all types of projectiles.
/// </summary>

public class RangedBehaviour : EntityBehaviour, Freezable
{
    protected Player player;
    public RangedData rangedData;
    protected Animator _animator;
    public GameObject _firer;
    private Rigidbody2D _rb;
    protected GameObject _target;
    protected Vector2 currenttargetposition;
    protected BoxCollider2D _collider;
    protected TrailRenderer _trailRenderer;
    protected RuntimeAnimatorController runtimeAnimator;
    private bool alreadyAttacked;

    [Header("Bullet properties")]
    [SerializeField] private float speed = 6.0f;
    private float lifeTime;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 200.0f;

    /** The first instance the gameobject is being activated.
    *  Retrieves all relevant data.
    */
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorControllerStateOnDisable = false;
        runtimeAnimator = _animator.runtimeAnimatorController;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        audioSource.volume = 0.5f;
        //_trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    /**
     * For general items which is used in all entities.
     */
    protected virtual void Start()
    {
    }

    /**
     * Reduce rangedbehaviour lifespan every frame.
     */
    private void Update()
    {
        ReduceLifeSpan();
    }

    /**
     * Move rangedbehaviour towards target every frame.
     */
    protected virtual void FixedUpdate()
    {
        if (rangedData._type != EntityData.TYPE.CAST_ONTARGET)
        {
            ShootAtTarget();
        }


    }

    /** OnEnable method.
     *  To intialize more specific ranged behaviours for ObjectPooling.
     */
    protected override void OnEnable()
    {
        base.OnEnable();
        DisableAnimator();
        SettingUpCollider();
        EnableAnimator();
        EnableAnimation();
        //_trailRenderer.enabled = true;
        transform.localScale = new Vector2(rangedData.scale, rangedData.scale);
        ResttingColor();
        rotateSpeed = rangedData.rotation;
        speed = rangedData.speed;
        if (_target != null)
        {
            gameObject.layer = _target.GetComponent<Player>() == null ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("EnemyProjectile");
        }
        alreadyAttacked = false;
        spriteRenderer.sortingOrder = 4;

    }


    //private void FixRotation()
    //{
    //    if (!rangedData.followTarget)
    //    {
    //        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //    }
       
    //}

    /**
     * Enabling of Animator.
     */
    private void EnableAnimator()
    {
        _animator.enabled = true;
        _animator.runtimeAnimatorController = runtimeAnimator;

    }

    /**
     * Disabling of Animator.
     */
    private void DisableAnimator()
    {
        _animator.enabled = false;
        _animator.runtimeAnimatorController = null;
    }

    /**
     * Resetting of color.
     */
    private void ResttingColor()
    {
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;
        //_trailRenderer.startColor = spriteRenderer.color;

    }

    /**
     * Resetting firer to null on disable.
     */
    protected void OnDisable()
    {
        _firer = null;

    }

    /**
     * Reduce Life Span of rangedbehaviour by time.deltatime.
     */
    private void ReduceLifeSpan()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Defeated();
        }
    }

    /**
     * Setting Target for the rangedbehaviour.
     */
    public void TargetEntity(GameObject target)
    {
        _target = target;
        currenttargetposition = target.transform.position;
        if (!_target.CompareTag("Player"))
        {
            lifeTime = 999f;
        }
        else
        {
            lifeTime = rangedData.lifetime;
        }
    }

    /**
     * Setting Up Collider for rangedbehaviour.
     */
    private void SettingUpCollider()
    {
        if (rangedData._type == EntityData.TYPE.PROJECTILE)
        {
            _collider.isTrigger = false;

        }
        else
        {
            transform.position = rangedData._type == EntityData.TYPE.CAST_ONTARGET ? player.transform.position : transform.position;
            _collider.isTrigger = true;
        }

        _collider.size = rangedData.sprite.bounds.size; /* rangedData.scale*/
        _collider.offset = Vector2.zero;
 
    }

    /**
     * Enabling animation.
     */
    public void EnableAnimation()
    {
        //_animator.applyRootMotion = true;
        if (rangedData.loop)
        {
            if (rangedData.trigger != "")
            {
                _animator.SetBool(rangedData.trigger, true);
            }
        }
        else
        {
            _animator.SetTrigger(rangedData.trigger);
        }

    }


    /**
     * Disabling animation.
     */
    private void DisableAnimation()
    {
        if (rangedData.loop && rangedData.trigger != "")
        {
            _animator.SetBool(rangedData.trigger, false);
        }

    }

    /**
     * Setting rangedbehaviour stats.
     */
    public override void SetEntityStats(EntityData stats)
    {
        RangedData rangedD = (RangedData)stats;
        if (rangedD != null)
        {
            this.rangedData = rangedD;
        }
    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {
        //if (_firer != null)
        //{
        //    EnemyBehaviour enemy = _firer.GetComponent<EnemyBehaviour>();
        //    if (!enemy.insideStage2 && enemy.currstate != StateMachine.STATE.RECOVERY)
        //    {
        //        enemy.resetCooldown();
        //    }
      
        //}
        poolManager.ReleaseObject(this);

    }

    /**
     * Retrieiving rangedbehaviour data.
     */
    public override EntityData GetData()
    {
        return this.rangedData;
    }

    /**
     * Play audio of rangedbehaviour.
     */
    public void PlayAudio()
    {
        if(rangedData.attackAudios.Count > 0)
        {
            audioSource.pitch = 1f;
            audioSource.clip = rangedData.attackAudios[0];
            audioSource.Play();
        }
        
    }

    //private IEnumerator WaitForAudio()
    //{
    //    if (rangedData.attackAudios.Count > 0)
    //    {
    //        yield return StartCoroutine(LoadSingleAudio(rangedData.attackAudios[0]));
    //    }
    //    yield return null;
        
    //}

    /**
     * Apply damage for current rangedbehaviour.
     */
    private void ApplyDamage(GameObject go)
    {
        if (ReferenceEquals(go, _target))
        {
            
            if (go.CompareTag("Player"))
            {
                Player player = go.GetComponent<Player>();
                if (!player.IsDead())
                {
                    player.TakeDamage(rangedData.damage);
                }

            }
            else
            {
                EnemyBehaviour enemy = go.GetComponentInChildren<EnemyBehaviour>();
                

                if (enemy != null)
                {
                    enemy.TakeDamage(rangedData.damage);
                    
                }
            }
        }
    }

    /**
     * Check if collision is player or an enemy.
     * Will only be called by bullet types since trigger will be set to true for spell objects.
     */
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        //_trailRenderer.enabled = false;

        if (!alreadyAttacked)
        {
            DisableAnimation();
            if (rangedData.impact_trigger != "")
            {
                _animator.SetBool(rangedData.impact_trigger, true);
            }
            else
            {
                Defeated();

            }
            stopMovement();
            alreadyAttacked = true;
            GameObject go = collision.gameObject;
            ApplyDamage(go);

            
        }


    }

    /**
     * Check if collision is player or an enemy.
     * Will only be called by spell types since trigger will be set to true for spell objects.
     */
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Setting collision to true so that it will trigger the next state
        //for bullet and thus play the explosion animation
        GameObject go = collision.gameObject;
        if (rangedData._type != EntityData.TYPE.PROJECTILE)
        {
            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                ApplyDamage(go);
                ApplyForce(go);
            }
            
        }


    }

    /**
     * Apply Force to target on hit.
     */
    private void ApplyForce(GameObject go)
    {
        if (ReferenceEquals(go, _target))
        {
            Vector2 dir = (Vector2)(_target.transform.position - transform.position).normalized;
            go.GetComponent<Rigidbody2D>().AddForce(dir * (rangedData.speed * 0.1f), ForceMode2D.Impulse);
        }
        else
        {
            stopMovement();
        }

    }

    /**
     * Stop movement on hit.
     */
    private void stopMovement()
    {
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    /**
     * Applying force to rangedbehaviour towards target/current direction.
     */
    protected void ShootAtTarget()
    {
        Vector2 targetpos = rangedData.followTarget ? _target.transform.position : currenttargetposition;
        Vector2 point2Target = (Vector2)transform.position - targetpos;
        point2Target.Normalize();
        Vector3 shootpoint = rangedData._type == EntityData.TYPE.PROJECTILE ? transform.right : -point2Target;
        //float value = Vector3.Cross(point2Target, transform.right).z;
        //_rb.angularVelocity = rotateSpeed * value;
        //_rb.velocity = transform.right * speed;
        float value = Vector3.Cross(point2Target, transform.right).z;
        
        if (rangedData.followTarget)
        {
            _rb.angularVelocity = rotateSpeed * value;
            _rb.velocity = shootpoint * speed;
        }
        else
        {
            _rb.angularVelocity = 0f;
            _rb.AddForce(shootpoint, ForceMode2D.Impulse);

        }

    }

    /**
     * Freeze rangedbehaviour.
     */
    public void Freeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.speed = 0;
    }

    /**
     * Unfreeze rangedbehaviour.
     */
    public void UnFreeze()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _animator.speed = 1;
    }
}
