using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    //animation handler;
    public enum ANIMATION_CODE
    {
        ATTACK_END,
        CAST_END,
        ATTACK_TRIGGER,
    }

    //data;
    public EntityStats stats;
    public StateMachine stateMachine;
    private int cooldown;
    public SpriteRenderer spriteRenderer { get; private set; }

    [Header("Entity Position")]
    public Vector2 roamPos;
    public float maxDist = 5f;

    public MeleeComponent melee { get; private set; }
    public RangedComponent ranged { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGO { get; private set; }
    public DetectionScript detectionScript;
    public Vector2 startingpos;

    public virtual void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        melee = GetComponentInChildren<MeleeComponent>();
        ranged = GetComponentInChildren<RangedComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingpos = GetComponent<Transform>().position;
        cooldown = 0;

    }

    public virtual void Update()
    {
        stateMachine.Update();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacles" || collision.gameObject.tag == "Enemy")
        {
            getNewRoamPosition();
        }
    }

    //public void SetVelocityRoam()
    //{
    //    rb.velocity = rb.velocity * stats.moveSpeed;

    //}

    //public void SetVelocityChase()
    //{
    //    rb.velocity = rb.velocity * stats.chaseSpeed;
    //}

    public void getNewRoamPosition()
    {
        roamPos = new Vector2(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
        
    }

    public void moveToRoam()
    {
        float steps = stats.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, roamPos, steps);
        flipFace(roamPos);
    }

    public void moveToTarget(Player player)
    {
        float steps = stats.chaseSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,player.transform.position,steps);
        flipFace(player.transform.position);
    }

    public void moveToStartPos()
    {
        float steps = stats.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, startingpos, steps);
        flipFace(startingpos);
    }

    public bool inMeleeRange()
    {
        return melee.inRange();
    }

    public bool inCastRange()
    {
        if (ranged == null)
        {
            return false;
        }
        return ranged.inRange();
    }


    public bool isReached()
    {
        return Vector2.Distance(roamPos,transform.localPosition) == 0;
    }

    public bool onCooldown()
    {
        return this.cooldown != 0;
    }

    public void resetCooldown()
    {
        this.cooldown = 200;
    }

    public void flipFace(Vector2 target)
    {
        Vector2 dir = (target - (Vector2) transform.position).normalized;
        spriteRenderer.flipX = dir.x > 0;
        foreach (Transform tf in transform)
        {
            if (tf.tag == "UI")
            {
                continue;
            }
            else if (dir.x > 0)
            {
                tf.localScale = new Vector2(-1,1);
            } else
            {
                tf.localScale = new Vector2(1, 1);
            }
            
        }

    }

    public void meleeAttack()
    {
        GameObject go = melee.detectionScript.playerDetected;
        if (go != null)
        {
            Player player = go.GetComponent<Player>();
            melee.Attack(player);
        }
    }

    public void AnimationBreak(ANIMATION_CODE code)
    {
        switch (code)
        {
            case ANIMATION_CODE.ATTACK_TRIGGER:
                meleeAttack();
                break;
            case ANIMATION_CODE.ATTACK_END:
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                break;
            case ANIMATION_CODE.CAST_END:
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                break;
                     
        }
    }

    public void Hurt()
    {
        Debug.Log(animator);
        animator.SetTrigger("Hurt");
    }

    public void Defeated()
    {
        StartCoroutine(Wait());
        animator.SetTrigger("Death");
    }


    //for enemies without a death state.
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    // use for enemies with death default states.
    private void DisableEnemy()
    {
        foreach(Transform tf in transform.GetComponentsInChildren<Transform>())
        {
            //disable colliders
            Collider2D[] cols = tf.GetComponents<Collider2D>();
            foreach(Collider2D col in cols)
            {
                col.enabled = false;
            }

            //disable scripts;
            MonoBehaviour[] scripts = tf.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour script in scripts)
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


}