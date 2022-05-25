using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextLogic : TextLogic{
    // Start is called before the first frame update
    WordBank wordGenerator;

    protected override void Awake()
    {

        base.Awake();
        minDist = 3f;
        wordGenerator = GameObject.FindObjectOfType<WordBank>();

    }

    protected override bool CheckInternalInput()
    {
        return !parent.gameObject.GetComponent<EnemyBehaviour>().stateMachine.Equals(StateMachine.STATE.STOP);
    }

    protected override void GenerateNewWord()
    {
        currentword = wordGenerator.GetWord();
    }
}
