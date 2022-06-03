using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RangedBehaviour
{
    public EnemyBehaviour _firer;
    private Rigidbody2D _rb;
    protected EntityBehaviour _target;
    protected string animationname;

    [Header("Bullet properties")]
    private float speed;
    private float lifeTime;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 200.0f;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        //this.spriteRenderer.sprite = rangedData.sprite;
        //_animator = (Animator) Resources.Load(string.Format("Animations/AnimatorControllers/{0}", rangedData.ac_name));
        //_animator = GetComponent<Animator>();
        //_animator.enabled = false;
        
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        speed = rangedData.speed;
        spriteRenderer.sprite = rangedData.sprite;
        if (!_target.CompareTag("Player"))
        {
            lifeTime = 999f;
        }
        else
        {
            lifeTime = rangedData.lifetime;
        }
        gameObject.AddComponent<Animator>();
        _animator = GetComponent<Animator>();
        animationname = rangedData.trigger;
        _animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", rangedData.ac_name)) as RuntimeAnimatorController;
        Debug.Log(_animator);
        transform.localScale = new Vector2(rangedData.scale, rangedData.scale);
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        speed = rangedData.speed;
    }


    /** can merge using Transform.
     */

    public void TargetEntity(EntityBehaviour entity)
    {
        this._target = entity;
    }

    protected virtual void FixedUpdate()
    {
        //When the bullet collide with the enemy, stop the movement of the bullet
        stopMovement(rangedData.trigger);
        //The bullet will follow the target
        followTarget();
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Setting collision to true so that it will trigger the next state
        //for bullet and thus play the explosion animation
        _animator.SetBool(animationname, true);
        if (_firer != null)
        {
            _firer.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        }
        EntityBehaviour _hit = collision.gameObject.GetComponent<EntityBehaviour>();

        if (_hit == null || _hit != _target) // 
        {
            return;
        }
        if (_hit.CompareTag("Player"))
        {
            _hit.GetComponent<Player>().TakeDamage(rangedData.damage);
        }
        else
        {
            _hit.Defeated();
        }
        
    }

    protected void stopMovement(string animationname)
    {
        if (_animator.GetBool(animationname) == true)
        {
            _rb.angularVelocity = 0;
            _rb.velocity = Vector2.zero;
            return;
        }
    }

    protected void followTarget()
    {
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        _rb.angularVelocity = rotateSpeed * value;
        _rb.velocity = transform.right * speed;
    }
}
