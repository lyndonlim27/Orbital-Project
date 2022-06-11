using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextLogic : TextLogic{
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
        return (!stateMachine.currState.Equals(StateMachine.STATE.STOP)) && (!stateMachine.currState.Equals(StateMachine.STATE.RECOVERY));
    }

    protected override void GenerateNewWord()
    {
        currentword = "a";
        //currentword = wordGenerator.GetWord();
        UpdateRemainingWord(currentword);
    }

    protected override void PerformAction()
    {
        Debug.Log("This is the target gameobject: " + parent.gameObject);
        player.Shoot(parent.gameObject);
    }
}
