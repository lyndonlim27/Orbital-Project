using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMeleeFodder : EnemyBehaviour
{
    private float Dashcooldown;
    private float Teleportcooldown;
    private bool dashStarted;
    private bool dashFinished;
    private float UltCooldown;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new C_MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        Dashcooldown = 15f;
        Teleportcooldown = 30f;
        UltCooldown = 30f;
    }

    public override void Update()
    {
        base.Update();
        Dodge();

    }


    public void resetUltCooldown()
    {
        UltCooldown = 30f;
        inAnimation = false;

    }

    public void Dodge()
    {
        if (Dashcooldown <= 0 && !inAnimation)
        {
            List<string> dodges = enemyData.defends;
            if(dodges.Count != 0)
            {
                int random = Random.Range(0, dodges.Count);
                animator.SetTrigger(dodges[random]);
                DisableAttackComps();
                inAnimation = true;
            }
            
        }
        else
        {
            Dashcooldown -= Time.deltaTime;
        }
    }

    private void DisableAttackComps()
    {
        if (melee != null)
        {
            melee.gameObject.SetActive(false);
        }

        if (ranged != null)
        {
            ranged.gameObject.SetActive(false);
        }
       
        
    }

    private void EnableAttackComps()
    {
        if (melee != null)
        {
            melee.gameObject.SetActive(enemyData.meleetriggers.Count > 0);
        }
        if (ranged != null)
        {
            ranged.gameObject.SetActive(enemyData.rangedtriggers.Count > 0);
        }
        
    }

    public void ResetDash()
    {
        Dashcooldown = 15f;
        inAnimation = false;
        //transform.parent.position = transform.position;
        //transform.localPosition = Vector3.zero;
        resetPosition();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        EnableAttackComps();
    }

    public override void Teleport()
    {
        getNewRoamPosition();
        transform.parent.position = roamPos;
        transform.localPosition = Vector3.zero;
        Dashcooldown = 30f;
        inAnimation = false;
        EnableAttackComps();
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    //public void CastSpell()
    //{
    //    List<string> spellcasts = enemyData.rangedtriggers;
    //    int rand = Random.Range(0, spellcasts.Count);
    //    GetComponent<Animator>().Play(spellcasts[rand]);
    //}
}
