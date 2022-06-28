using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterS : EnemyBehaviour
{

    
    public Vector2 castingpos;
    public GameObject dice;
    public int roll;
    public bool playing;
    public bool transformed;

    /* not yet implemented.
     */
    private List<EnemyBehaviour> fodderObjects;
    public HealthBarEnemy hpBarUI;
    public float HardenCooldown = 0;
    public BossProps stageProp;
    public List<ItemWithTextData> propsData;
    public List<BossProps> allProps;
    public EnemyData[] fodderData;
    public EnemyBehaviour fodderType;



    protected override void Awake()
    {
        base.Awake();
        hpBarUI = GameObject.FindObjectOfType<HealthBarEnemy>(true);
        hpBarUI.gameObject.SetActive(true);
        fodderObjects = new List<EnemyBehaviour>();
        allProps = new List<BossProps>();
        stateMachine = new StateMachine();
        transformed = false;

    }

    public void Start()
    {
       
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new S_DiceState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new S_KnightState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new S_MageState(this, this.stateMachine));
        //stateMachine.AddState(StateMachine.STATE.ENRAGED1, new HardenStage(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        castingpos = new Vector3(0, 0, 0);
    }

    public override void Update()
    {
        base.Update();
    }


    public override void Initialize()
    {
        this.animator.SetBool("NotSpawned", true);
        this.animator.SetBool("isAlive", true);
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);

    }

    public override bool hasWeapon()
    {
        return base.hasWeapon();
    }

    //public override void Defeated()
    //{
    //    if (hpBarUI.currlength == 0)
    //    {
    //        isDead = true;
    //        animator.SetTrigger("Death");
    //        hpBarUI.gameObject.SetActive(false);
    //        Debug.Log(hpBarUI.currlength);

    //    }
    //    else
    //    {
    //        Hurt();
    //    }
    //}


    private void SpawnProps()
    {
        foreach (ItemWithTextData data in propsData)
        {
            stageProp.SetEntityStats(data);
            BossProps bossprop = Instantiate(stageProp, data.pos, Quaternion.identity);
            bossprop.SetCurrentRoom(this.currentRoom);
            bossprop.SetBoss(this);
            allProps.Add(bossprop);
        }
    }


    public void SpawnFodders()
    {
        foreach (EnemyData fod in fodderData)
        {
            fodderType.SetEntityStats(fod);
            EnemyBehaviour fodder = Instantiate(fodderType, fod.pos, Quaternion.identity);
            fodder.SetCurrentRoom(this.currentRoom);
            fodderObjects.Add(fodder);
        }
    }

    public void DestroyFodders()
    {
        foreach (EnemyBehaviour fod in fodderObjects)
        {
            if (fod != null)
            {
                Destroy(fod.gameObject);
            }
        }
    }

    public void DestroyBossProps()
    {
        foreach (BossProps bp in allProps)
        {
            if (bp != null)
            {
                Destroy(bp.gameObject);
            }

        }
    }

    public override void AnimationBreak(ANIMATION_CODE code)
    {
        switch (code)
        {
            case ANIMATION_CODE.ATTACK_END:
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                break;
            case ANIMATION_CODE.MELEE_TRIGGER:
                meleeAttack();
                break;
            case ANIMATION_CODE.CAST_END:
                //animator.SetTrigger("TeleportIn");
                //transform.parent.position = castingpos;
                stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
                break;
        }
    }

    public void TriggerTeleport()
    {
        animator.SetTrigger("TeleportOut");
    }

    public void TeleportOut()
    {
        if (animator.GetBool("MageState"))
        {
            getNewRoamPosition();
            transform.parent.position = roamPos;
        } else
        {
            transform.parent.position = player.transform.position;
        }
        //Debug.Log(stateMachine.currState);
        //Debug.Break();

    }

    public IEnumerator TransformKnight(int rand)
    {

        animator.SetBool("isWalking", false);
        //animator.SetBool("MageState", false);
        animator.SetTrigger("TeleportOut");
        yield return new WaitForSeconds(2f);
        //then after teleporting in, we transform to knight;
        animator.SetTrigger("TransformKnight");
        yield return new WaitForSeconds(1f);
        //animator.SetBool("MageState", false);
        stateMachine.ChangeState(StateMachine.STATE.ATTACK1, rand);
    }

    public IEnumerator TransformMage(int rand)
    {
        animator.SetBool("isWalking", false);
        
        animator.SetTrigger("TransformToMage");
        yield return new WaitForSeconds(3.5f);
        //animator.SetBool("MageState", true);
        //then after teleporting in, we transform to knight;
        stateMachine.ChangeState(StateMachine.STATE.ATTACK2, rand);
    }

    public IEnumerator TeleportMage(int rand)
    {
        animator.SetBool("isWalking", false);
        animator.SetTrigger("TeleportOut");
        yield return new WaitForSeconds(2f);
        stateMachine.ChangeState(StateMachine.STATE.ATTACK2, rand);
        
        
    }


    public IEnumerator triggerAttack(int roll)
    {
        if (player.IsDead())
        {
            stateMachine.ChangeState(StateMachine.STATE.STOP, null);
        }

        animator.SetBool("isWalking", false);

        dice.GetComponent<Animator>().SetTrigger(string.Format("Roll{0}", roll));
        yield return new WaitForSeconds(1.5f);
        switch (roll)
        {
            //find another spell ltr;
            //case 1:
            //    if (enemy.melee.detected())
            //    {
            //        enemy.animator.SetTrigger("Melee1");
            //    }

            //    break;
            default:
                flipFace((player.transform.position));
                //ranged.StartCoroutine(
                    //ranged.Cast(ranged.rangeds[roll - 1]));

                break;
            case 3:
                flipFace((player.transform.position));
                animator.SetTrigger("Melee2");
                break;

        }
        yield return new WaitForSeconds(.5f);
        playing = false;
        
        
    }

    public void ChangeState()
    {
        animator.SetBool("MageState", !animator.GetBool("MageState"));
    }

}
