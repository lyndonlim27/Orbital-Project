using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DetectionScript),typeof(Animator))]
public class TrapBehaviour : ActivatorBehaviour
{
    StateMachine stateMachine;
    RangedComponent ranged;
 
    protected override void Awake()
    {
        base.Awake();
        detectionScript = GetComponentInChildren<DetectionScript>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animations/AnimatorControllers/AC_Trap") as RuntimeAnimatorController;
        ranged = GetComponentInChildren<RangedComponent>();


    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.TRAPINACTIVE, new TrapInActiveState(this, stateMachine));
        stateMachine.AddState(StateMachine.STATE.TRAPACTIVE, new TrapActiveState(this, stateMachine));
        stateMachine.Init(StateMachine.STATE.TRAPINACTIVE, null);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public void ActivateRangedComponents()
    {
        if (ranged != null)
        {
            ranged.ShootSingle();
        }
    }

    public void resetAnimation()
    {
        inAnimation = false;
    }

    public override void SetEntityStats(EntityData stats)
    {
        SwitchData temp = (SwitchData)stats;
        if(stats != null)
        {
            data = temp;
        }
    }

    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }

    public override EntityData GetData()
    {
        return data;
    }
}
