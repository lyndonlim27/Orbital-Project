using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{

    //animation handler;
    public enum ANIMATION_CODE
    {
        ATTACK_END,
        CAST_END,
        MELEE_TRIGGER,
        HURT_END,
        WEAP_TRIGGER
    }

    //data;
    public EnemyData enemyData;
    public StateMachine stateMachine;
    public int angerMultiplier = 1;
    private int cooldown;
    protected DamageFlicker _flicker;
    public bool isDead { get; private set; }

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
    public Player player;
    public int health;
    

    public virtual void Start()
    {
        GameObject go = GameObject.FindWithTag("Player");
        if (go != null)
        {
            player = go.GetComponent<Player>();
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}",stats.animatorname)) as RuntimeAnimatorController;
        animator.SetBool("Death", false);
        melee = GetComponentInChildren<MeleeComponent>();
        ranged = GetComponentInChildren<RangedComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyData.sprite;
        startingpos = GetComponent<Transform>().position;
        _flicker = GetComponent<DamageFlicker>();
        cooldown = 0;
        isDead = false;
        health = enemyData.words;
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
        if (collision.gameObject.tag == "Obstacles")
        {
            stateMachine.ChangeState(StateMachine.STATE.ROAMING,null);
        }
    }

    public void getNewRoamPosition()
    {
        roamPos = new Vector2(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
        
    }

    public void moveToRoam()
    {
        float steps = enemyData.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, roamPos, steps);
        flipFace(roamPos);
    }

    public void moveToTarget(Player player)
    {
        float steps = enemyData.chaseSpeed * Time.deltaTime * angerMultiplier;
        transform.position = Vector3.MoveTowards(transform.position,player.transform.position,steps);
        flipFace(player.transform.position);
    }

    public void moveToStartPos()
    {
        float steps = enemyData.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, startingpos, steps);
        flipFace(startingpos);
    }

    public bool isReached()
    {
        return Vector2.Distance(roamPos,transform.localPosition) == 0;
    }

    public bool onCooldown()
    {
        return this.cooldown != 0;
    }

    public void tick()
    {
        if (cooldown > 0)
        {
            cooldown--;
        }
    }

    public void resetCooldown()
    {
        this.cooldown = 250;
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
                meleeAttack();
                break;
            case ANIMATION_CODE.ATTACK_END:
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                break;
            case ANIMATION_CODE.CAST_END:
                resetCooldown();
                Debug.Log("This is inside golem state");
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                break;
            case ANIMATION_CODE.WEAP_TRIGGER:
                WeapAttack();
                break;
        }
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
        _flicker.Flicker();
        health -= 1; // or use weapon damage;
        Debug.Log(health);
        
    }

    public override void Defeated()
    {
        //StartCoroutine(Wait());
        if (health == 0)
        {
            isDead = true;
            animator.SetBool("Death", true);
        } else
        {
            Hurt();
        }
        
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

    public override void SetEntityStats(EntityData stats)
    {
        this.enemyData = (EnemyData) stats;
    }

    public override EntityData GetData()
    {
        return enemyData;
    }
}
