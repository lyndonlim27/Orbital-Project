using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StateMachine
{
    public enum STATE
    {
        //put all kinds of state here
        IDLE,
        MOVE,
        ATTACK

    }
    STATE currState;
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
}


