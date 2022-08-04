using UnityEngine;
using System;

namespace EntityCores.StateMachines
{
    public abstract class StateClass
    {
        protected EnemyBehaviour enemy;
        protected TrapBehaviour trap;
        protected StateMachine stateMachine;
        protected object stateData;
        public StateClass(EnemyBehaviour entity, StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            this.enemy = entity;
        }

        public StateClass(TrapBehaviour entity, StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            this.trap = entity;
        }

        virtual public void Enter(object stateData) { }
        // for logic related updates
        virtual public void Update() { }
        // for physics related updates;
        virtual public void FixedUpdate() { }
        virtual public void Exit() { }

    }
}