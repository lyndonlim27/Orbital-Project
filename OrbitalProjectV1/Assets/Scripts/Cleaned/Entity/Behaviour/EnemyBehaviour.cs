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
    //private int cooldown;
    protected DamageFlicker _flicker;
    public bool isDead;

    [Header("Entity Position")]
    public Vector2 roamPos;
    public float maxDist = 100f;

    public MeleeComponent melee { get; private set; }
    public RangedComponent ranged { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
 
    public DetectionScript detectionScript;
    public Vector2 startingpos;
    public Player player;
    public int health;

    public float cooldown;

    public virtual void Start()
    {

        player = GameObject.FindObjectOfType<Player>(true);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}",stats.animatorname)) as RuntimeAnimatorController;
        melee = GetComponentInChildren<MeleeComponent>();
        ranged = GetComponentInChildren<RangedComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemyData.sprite;
        startingpos = GetComponent<Transform>().position;
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        isDead = false;
        health = enemyData.words;
    }

    public void Initialize()
    {
        this.animator.SetBool("NotSpawned", true);
        stateMachine.Init(StateMachine.STATE.IDLE, null);
    }

    public virtual void Update()
    {
        stateMachine.Update();
      //  Debug.Log(cooldown);
        //Debug.Log(stateMachine.currState);
        //Debug.Log(cooldown);
    }

    public virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            getNewRoamPosition();
            stateMachine.ChangeState(StateMachine.STATE.ROAMING,null);
        }
    }


    public bool onCooldown()
    {
        if (ranged == null)
        {
            return this.cooldown > 0;
        } else
        {
            return this.cooldown > 0 && ranged.abletoAttack == true;
        }
        
    }

    public void tick()
    {
        if (cooldown > 0)
        {
            cooldown-= Time.deltaTime;
        }
    }



    public void resetCooldown()
    {
        this.cooldown = 20;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void getNewRoamPosition()
    {
        //roamPos = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        roamPos = this.currentRoom.GetRandomPoint();
 
        
    }

    public void moveToRoam()
    {
        float steps = enemyData.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, roamPos, steps);
        flipFace(roamPos);
    }

    public void moveToTarget(Player player)
    {
        float steps = enemyData.chaseSpeed * Time.deltaTime;
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

    public void flipFace(Vector2 target)
    {
        Vector2 dir = (target - (Vector2) transform.position).normalized;
        spriteRenderer.flipX = dir.x > 0.1f;
        if (melee == null)
        {
            return;
        }
        if (dir.x >= 0.1f)
        {
            melee.transform.localScale = new Vector2(-1, 1);
        }
        else if (dir.x <= 0.1f)
        {
            melee.transform.localScale = new Vector2(1, 1);
        }


    }

    public void Teleport()
    {
        this.transform.position = roamPos;
        this.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
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
                stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
                break;
            case ANIMATION_CODE.WEAP_TRIGGER:
                WeapAttack();
                break;
        }
    }

    public virtual void Hurt()
    {
        _flicker.Flicker(this);
        if (health == 0)
        {
            Defeated();
        } else
        {
            health--;
        }
        //hpBarUI.TakeDamage(); // or use weapon damage;
        
    }

    public override void Defeated()
    {
        //StartCoroutine(Wait());
        if (health == 0)
        {
            isDead = true;
            animator.SetTrigger("Death");
        } else
        {
            Hurt();
        }
        
    }



    //private void OnDestroy()
    //{
    //    Destroy(gameObject);
    //}

    //default fade animation;
    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        this.gameObject.SetActive(false);
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

    public RoomManager GetCurrentRoom()
    {
        return this.currentRoom;
    }
    
}
