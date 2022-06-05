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


    public override void resetCooldown()
    {
        cooldown = 5f;
        inAnimation = false;
       

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
            int random = Random.Range(0, dodges.Count);
            
            animator.SetTrigger(dodges[random]);
            inAnimation = true;
        }
        else
        {
            Dashcooldown -= Time.deltaTime;
        }
    }

    public void ResetDash()
    {
        Dashcooldown = 15f;
        inAnimation = false;
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public override void Teleport()
    {
        getNewRoamPosition();
        transform.parent.position = roamPos;
        Dashcooldown = 30f;
        inAnimation = false;
        stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
    }

    public void CastSpell()
    {
        List<string> spellcasts = enemyData.spelltriggers;
        int rand = Random.Range(0, spellcasts.Count);
        GetComponent<Animator>().Play(spellcasts[rand]);
    }
}
