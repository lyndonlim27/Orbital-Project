internal class StageOneState : StateClass
{
    public StageOneState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
        // at this stage we introduce more mechanics. when hp drop below 50%, enter this mode.
        // in this mode we just double down on all both stats.
    }


    public override void Enter(object stateData)
    {
        entity.angerMultiplier = 2;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

}