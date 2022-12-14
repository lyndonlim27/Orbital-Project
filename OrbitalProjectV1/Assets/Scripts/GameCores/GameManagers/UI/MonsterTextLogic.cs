using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCores.StateMachines;

namespace EntityCores
{
    public class MonsterTextLogic : TextLogic
    {
        // Start is called before the first frame update
        WordBank wordGenerator;

        protected override void Awake()
        {

            base.Awake();
            wordGenerator = GameObject.FindObjectOfType<WordBank>();

        }

        protected override bool CheckInternalInput()
        {
            StateMachine stateMachine = parent.gameObject.GetComponent<EnemyBehaviour>().stateMachine;
            if (stateMachine != null)
            {
                return (!stateMachine.currState.Equals(StateMachine.STATE.STOP)) && (!stateMachine.currState.Equals(StateMachine.STATE.RECOVERY));
            }
            else
            {
                return true;
            }

        }

        protected override void GenerateNewWord()
        {

            currentword = wordGenerator.GetWord();
            UpdateRemainingWord(currentword);


            //currentword = wordGenerator.GetWord();

        }

        protected override void PerformAction()
        {
            player.Shoot(parent.gameObject);
        }
    }
}
