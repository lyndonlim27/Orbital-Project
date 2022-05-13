using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class StateClass
{
    public GameObject gameObject;
    public StateMachine stateMachine;
    public StateClass(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
    }
    virtual public void Enter(object data) { }
    virtual public void FixedUpdate() { }
    virtual public void Update() { }
    virtual public void Exit() { }

}