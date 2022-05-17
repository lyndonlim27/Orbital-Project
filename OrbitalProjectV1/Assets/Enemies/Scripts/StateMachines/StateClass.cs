using UnityEngine;

public abstract class StateClass 
{
    protected Entity entity;
    protected StateMachine stateMachine;
    protected object stateData;
    public StateClass(Entity entity, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.entity = entity;
    }
    virtual public void Enter(object stateData) { }
    // for logic related updates
    virtual public void Update() { }
    // for physics related updates;
    virtual public void FixedUpdate() { }
    virtual public void Exit() { }

}