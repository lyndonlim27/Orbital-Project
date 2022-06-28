using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private static int count = 0;
    private bool alreadyAttacked;

    [Header("Bullet properties")]
    [SerializeField] private float speed = 6.0f;
    private float lifeTime;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 200.0f;

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

    protected virtual void Start()
    {
        //_collider.size = spriteRenderer.sprite.bounds.size;
    }

    private void Update()
    {
        ReduceLifeSpan();
    }

    protected virtual void FixedUpdate()
    {
        if (rangedData._type != EntityData.TYPE.CAST_ONTARGET)
        {
            ShootAtTarget();
        }


    }

    protected void OnEnable()
    {
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
    private void FixRotation()
    {
        if (!rangedData.followTarget)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
       
    }

    private void EnableAnimator()
    {
        _animator.enabled = true;
        _animator.runtimeAnimatorController = runtimeAnimator;

    }

    private void DisableAnimator()
    {
        _animator.enabled = false;
        _animator.runtimeAnimatorController = null;
    }

    private void ResttingColor()
    {
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;
        //_trailRenderer.startColor = spriteRenderer.color;

    }

    protected void OnDisable()
    {
        _firer = null;

    }

    private void ReduceLifeSpan()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Debug.Log(lifeTime);
            Defeated();
        }
    }

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
        //_collider.enabled = true;


    }

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

    private void DisableAnimation()
    {
        if (rangedData.loop && rangedData.trigger != "")
        {
            _animator.SetBool(rangedData.trigger, false);
        }

    }

    public override void SetEntityStats(EntityData stats)
    {
        RangedData rangedD = (RangedData)stats;
        if (rangedD != null)
        {
            this.rangedData = rangedD;
        }
    }

    public override void Defeated()
    {
        if (_firer != null)
        {
            EnemyBehaviour enemy = _firer.GetComponent<EnemyBehaviour>();
            if (!enemy.insideStage2 && enemy.currstate != StateMachine.STATE.RECOVERY)
            {
                enemy.resetCooldown();
            }
            //enemy.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

        }
        poolManager.ReleaseObject(this);

    }

    public override EntityData GetData()
    {
        return this.rangedData;
    }
    /**
     * will only be called by bullet types since trigger will be set to true for spell objects.
     */
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        //_trailRenderer.enabled = false;
        if (!alreadyAttacked)
        {
            DisableAnimation();
            stopMovement();
            alreadyAttacked = true;
            GameObject go = collision.gameObject;
            ApplyDamage(go);
            
            if (rangedData.impact_trigger != "")
            {
                _animator.SetBool(rangedData.impact_trigger, true);
            }
            else
            {
                Defeated();

            }



        }
        

    }

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
                Debug.Log("did we enter trigger");
                ApplyDamage(go);
                ApplyForce(go);
            }
            
        }


    }

    protected void OnTriggerStay2D(Collider2D collision)
    {

        //GameObject go = collision.gameObject;
        ////Debug.Log(go);

        //if (rangedData._type != EntityData.TYPE.PROJECTILE)
        //{
        //    //ApplyDamage(go);
        //    //ApplyForce(go);
        //}

    }

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

    private void stopMovement()
    {
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

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


        //_rb.AddForce(shootpoint*speed, rangedData.followTarget ? ForceMode2D.Force : ForceMode2D.Impulse);
        //_rb.velocity = shootpoint* speed;
    }

    public void Freeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.speed = 0;
    }

    public void UnFreeze()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _animator.speed = 1;
    }
}
