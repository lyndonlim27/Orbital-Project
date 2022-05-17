using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected IWeapon weapon;
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool armed;
    protected float nextWaypointDistance = 1f;
    protected Vector3 startingPos;
    protected Vector3 roamPosition;
    protected Transform target;
    protected DetectionScript detector;
    protected StateMachine stateMachine;


    public enum ANIMATION_CODE
    {
        ATTACK_END,
        CAST_END,
        ATTACK_TRIGGER
    }

    //public abstract void Attack();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        detector = GetComponentInChildren<DetectionScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startingPos = transform.position;
        animator.SetBool("isWalking", true);
    }

    protected Vector3 GetRoamingPosition()
    {
        return (startingPos + UtilsClass.GetRandomDir() * Random.Range(1f, 10f));
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Obstacles"))
        {
            this.roamPosition = GetRoamingPosition();
        }
    }

    //public void AnimationTrigger(ANIMATION_CODE code)
    //{
    //    switch (code)
    //    {
    //        case ANIMATION_CODE.ATTACK_END:
    //            stateMachine.ChangeState(StateMachine.STATE.MOVE, MoveState.MOVETYPE.POSITION);
    //            break;
    //        case ANIMATION_CODE.CAST_END:
    //            stateMachine.ChangeState(StateMachine.STATE.MOVE, MoveState.MOVETYPE.POSITION);
    //            break;
    //        case ANIMATION_CODE.ATTACK_TRIGGER:
    //            Attack();
    //            //weapon.attack() better
    //            break;
    //    }
    //}


    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Defeated();
        } 
    }

    private void Defeated()
    {
        animator.SetBool("isAlive", false);
    }

    private void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    protected float Rand()
    {
        return Random.Range(1f, 1800f);

    }
}
