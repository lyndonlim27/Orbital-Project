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
    public int HardenCooldown = 0;
    public bool stage2;


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
        stateMachine.AddState(StateMachine.STATE.ENRAGED1, new HardenStage(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        
    }

    public override bool hasWeapon()
    {
        return true;
    }

    public override void Hurt()
    {
        _flicker.Flicker(this);
        hpBarUI.TakeDamage();
    }

    public override void Defeated()
    { 
        if (hpBarUI.currlength == 0)
        {
            isDead = true;
            animator.SetTrigger("Death");
            hpBarUI.gameObject.SetActive(false);
            Debug.Log(hpBarUI.currlength);

        }
        else
        {
            Hurt();
        }
    }

    public override void Update()
    {
        if (this.hpBarUI.QuarterHP() && !stage2)
        {
            animator.SetTrigger("Stage2");
        }

        else if (!stage2 && this.hpBarUI.HalfHP() && stateMachine.currState != StateMachine.STATE.ENRAGED1 && HardenCooldown == 0)
        {
            
            this.stateMachine.ChangeState(StateMachine.STATE.ENRAGED1, null);
           
        } else
        {
            base.Update();
            if (HardenCooldown > 0)
            {
                HardenCooldown--;
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
        if (stateMachine.currState == StateMachine.STATE.ENRAGED1)
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
        this.HardenCooldown = 8000;
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


