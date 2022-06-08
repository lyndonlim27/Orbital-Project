using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public enum STATE
    {
        IDLE,
        MOVE,
        CHASE,
        ROAMING,
        STOP,
        ATTACK1,
        ATTACK2,
        DEATH,
        RECOVERY,
        ENRAGED2,
        ENRAGED3,
        ENRAGED4,
        TELEPORT,

    }
    public STATE currState { get; private set; }
    Dictionary<STATE, StateClass> stateDict;
    public StateMachine()
    {
        stateDict = new Dictionary<STATE, StateClass>();
        currState = STATE.IDLE;
    }
    public void Init(STATE state, object stateData)
    {
        currState = state;
        stateDict[currState].Enter(stateData);
    }

    public void AddState(STATE state, StateClass concreteState)
    {
        stateDict[state] = concreteState;
    }
    public void ChangeState(STATE newState, object stateData)
    {
        stateDict[currState].Exit();
        currState = newState;
        stateDict[currState].Enter(stateData);
    }

    public void Update()
    {
        stateDict[currState].Update();
    }

    public void FixedUpdate()
    {
        stateDict[currState].FixedUpdate();
    }
}


