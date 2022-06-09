using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBehaviour : EntityBehaviour
{
    protected Player player;
    public RangedData rangedData;
    protected Animator _animator;
    public GameObject _firer;
    private Rigidbody2D _rb;
    protected GameObject _target;
    protected BoxCollider2D _collider;

    [Header("Bullet properties")]
    [SerializeField] private float speed = 6.0f;
    private float lifeTime;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 200.0f;

    protected virtual void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorControllerStateOnDisable = false;
        _rb = GetComponent<Rigidbody2D>();
        poolManager = FindObjectOfType<PoolManager>(true);
        _collider = GetComponent<BoxCollider2D>();
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

        if (rangedData.followTarget)
        {
            followTarget();
        } else
        {
            _rb.velocity = transform.forward * 200f;
        }

    }

    protected void OnEnable()
    {
        EnableAnimation();
        SettingUpCollider();
        transform.localScale = new Vector2(rangedData.scale, rangedData.scale);
        rotateSpeed = rangedData.rotation;
        speed = rangedData.speed;

    }

    protected void OnDisable()
    {
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;

    }

    private void ReduceLifeSpan()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void TargetEntity(GameObject target)
    {
        _target = target;
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
        Debug.Log("What stats is this ? : " + stats);
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
            Debug.Log("entered");
            enemy.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            enemy.resetCooldown();
        }
        Debug.Log(lifeTime);
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
        stopMovement();
        DisableAnimation();
        if (rangedData.impact_trigger != "")
        {
            _animator.SetBool(rangedData.impact_trigger, true);
        } 
        GameObject go = collision.gameObject;
        ApplyDamage(go);
        Defeated();

    }

    private void ApplyDamage(GameObject go)
    {
        
        if (ReferenceEquals(go, _target))
        {
            if (go.CompareTag("Player"))
            {
                Player player = go.GetComponent<Player>();
                if (!player.isDead()) {
                    player.TakeDamage(rangedData.damage);
                }
                
            }
            else
            {
                EnemyBehaviour enemy = go.GetComponent<EnemyBehaviour>();
                if (enemy != null)
                {
                    enemy.Defeated();
                }
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Setting collision to true so that it will trigger the next state
        //for bullet and thus play the explosion animation
        GameObject go = collision.gameObject;

        ApplyDamage(go);
        ApplyForce(go);

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        
        GameObject go = collision.gameObject;
        Debug.Log(go);

        ApplyDamage(go);
        ApplyForce(go);

    }

    private void ApplyForce(GameObject go)
    {
        if (ReferenceEquals(go, _target))
        {
            Vector2 dir = (Vector2)(_target.transform.position - transform.position).normalized;
            go.GetComponent<Rigidbody2D>().AddForce(dir * (rangedData.speed*0.1f), ForceMode2D.Impulse);
        } else
        {
            stopMovement();
        }
    }

    protected void stopMovement()
    {
        
        _rb.angularVelocity = 0;
        _rb.velocity = Vector2.zero;
        
    }

    protected void followTarget()
    {
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        Vector3 shootpoint = rangedData._type == EntityData.TYPE.PROJECTILE ? transform.right : -point2Target;
        //float value = Vector3.Cross(point2Target, transform.right).z;
        //_rb.angularVelocity = rotateSpeed * value;
        //_rb.velocity = transform.right * speed;
        float value = Vector3.Cross(point2Target, transform.right).z;
        _rb.angularVelocity = rotateSpeed * value;
        _rb.velocity = shootpoint* speed;
    }

}
