using UnityEngine;
using System;

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

    //handling null target;
    //public Player CheckTargetExistence()
    //{
    //    try
    //    {
    //        return entity.detectionScript.playerDetected.GetComponent<Player>()
    //    } catch (NullReferenceException err)
    //    {
    //        Console.WriteLine("Please check the string str.");
    //    }
    //    Player player;
    //    if (entity.detectionScript.playerDetected != null)
    //    {
    //        GameObject go = entity.detectionScript.playerDetected;
    //        if (go != null)
    //        {
    //            player = go.GetComponent<Player>();

    //        }
    //        else
    //        {
    //            player = null;
    //        }
    //    }

}