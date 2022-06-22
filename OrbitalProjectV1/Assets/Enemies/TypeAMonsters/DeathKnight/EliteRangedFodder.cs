using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRangedFodder : EnemyBehaviour
{
    private float Dashcooldown;
    private float Teleportcooldown;
    private bool dashStarted;
    private bool dashFinished;
    private float UltCooldown;
    // Start is called before the first frame update
    public void Start()
    {
        
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ROAMING, new RoamState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.CHASE, new ChaseState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK2, new RangedState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.STOP, new StopState(this, this.stateMachine));
        stateMachine.Init(StateMachine.STATE.IDLE, null);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("this is state: " + stateMachine.currState);
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

}
