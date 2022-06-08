using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterA : EnemyBehaviour
{
    public BossProps stageProp;
    public List<ItemWithTextData> propsData;
    public List<BossProps> allProps;
    public EnemyData[] fodderData;
    public EnemyBehaviour fodderType;
    private List<EnemyBehaviour> fodderObjects;
    public HealthBarEnemy hpBarUI;
    public float HardenCooldown = 0;


    private void Awake()
    { 
        hpBarUI = GameObject.FindObjectOfType<HealthBarEnemy>(true);
        hpBarUI.gameObject.SetActive(true);
        fodderObjects = new List<EnemyBehaviour>();
        allProps = new List<BossProps>();
    }

    public override void Start()
    {
        base.Start();
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.RECOVERY, new HardenStage(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));

    }


    public override void Hurt()
    {
        hpBarUI.TakeDamage();
        base.Hurt();
 
    }

    public override void Defeated()
    {
        base.Defeated();
        hpBarUI.gameObject.SetActive(false);
    }

    public override void Update()
    {

        if (this.hpBarUI.QuarterHP() && !stage2)
        {
            animator.SetTrigger("Stage2");
        }

        else if (!stage2 && health <= (0.5*enemyData.words) && stateMachine.currState != StateMachine.STATE.RECOVERY && HardenCooldown == 0)
        {
            
            this.stateMachine.ChangeState(StateMachine.STATE.RECOVERY, null);
           
        } else
        {
            stateMachine.Update();
            if (HardenCooldown > 0)
            {
                HardenCooldown -= Time.deltaTime;
            }
        }
    }

    public void ActivateStage1()
    {
        SpawnProps();
        SpawnFodders();
    }

    
    public void ActivateStage2()
    {
        stage2 = true;
        this.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void CheckInteruption()
    {
        if (stateMachine.currState == StateMachine.STATE.RECOVERY)
        {
            Debug.Log(allProps.Count);
            if (allProps.Count == 0 || hpBarUI.HPBarFull())
            {
                resetHardenCooldown();
            }
        }
    }

    public void resetHardenCooldown()
    {
        this.HardenCooldown = 80;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void RegenHP()
    {
        hpBarUI.RegainHealth();
    }

    private void SpawnProps()
    {
        foreach (ItemWithTextData data in propsData)
        {
            stageProp.SetEntityStats(data);
            BossProps bossprop = Instantiate(stageProp, data.pos, Quaternion.identity);
            bossprop.SetCurrentRoom(this.currentRoom);
            bossprop.SetBoss(this);
            allProps.Add(bossprop);
            Debug.Log(allProps.Count);
        }
    }


    public void SpawnFodders()
    {
        foreach(EnemyData fod in fodderData)
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
        Debug.Log(allProps.Count);
        foreach (BossProps bp in allProps)
        {
            if (bp != null)
            {
                Destroy(bp.gameObject);
            }
            
        }
    }

}


