using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : ItemWithTextBehaviour
{
    private LightSwitchSystem lightSwitchSystem;
    private PuzzleInputManager puzzleInputManager;
    private TextLogic puzzleTl;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        puzzleInputManager = FindObjectOfType<PuzzleInputManager>(true);
        Debug.Log("Puzzle input manager == " + puzzleInputManager);
        lightSwitchSystem = FindObjectOfType<LightSwitchSystem>(true);
        

    }
    
    public override void Defeated()
    { 
        PullUpPuzzleMenu();
    }

    private void PullUpPuzzleMenu()
    {
        Debug.Log("Did we pul up?");   
        ////stop lightshow when box is interacted;
        //StopCoroutine(lightSwitchSystem.StartLightShow());
        //pull up puzzlemenu;
        puzzleInputManager.SetUp(lightSwitchSystem.GetCurrentSeq().Count);
        puzzleInputManager.gameObject.SetActive(true);
        FindObjectOfType<SkillManager>().enabled = false;
        //set up current seq and numofcandles for setting letterslots
        
    }

    private void Start()
    {
        puzzleTl = GetComponentInChildren<TextLogic>();
        puzzleTl.enabled = false;
        lightSwitchSystem.Activate(1);
        StartCoroutine(lightSwitchSystem.StartLightShow());
    }

    private void Update()
    {
        ReceiveInput();
        CheckForNext();
        CheckComplete();
    }

    private void CheckForNext()
    {
        //checking for coroutine not really needed since i decided when the box is available to guess, but just in case
        if (lightSwitchSystem.activated && !lightSwitchSystem.incoroutine && puzzleInputManager.guessed)
        {
            if (CheckInput())
            {
                lightSwitchSystem.Next();
            }
            else
            {
                StartCoroutine(lightSwitchSystem.StartLightShow());
            }
        }
    }

    private void CheckComplete()
    {
        
        if (lightSwitchSystem.IsComplete())
        {
            Debug.Log("Complete" + currentRoom.conditions.Count);
            currentRoom.FulfillCondition(data._name + data.GetInstanceID());
            poolManager.ReleaseObject(this);
        }
    }

    private void ReceiveInput()
    {
        if (!lightSwitchSystem.incoroutine)
        {
            Debug.Log("allow textinputs???");
            puzzleTl.enabled = true;
        }
        else
        {
            puzzleTl.enabled = false;
        }
    }

    private bool CheckInput()
    {
        
        List<int> answer = lightSwitchSystem.GetCurrentSeq();
        LetterSlotNoDnD[] guess = puzzleInputManager.GetCurrentGuess();
        if (guess != null)
        {
            //return false;
            foreach (LetterSlotNoDnD d in guess)
            {
                Debug.Log("Current num is " + d.currnum);
            }

            for (int i = 0; i < answer.Count; i++)
            {
                if (guess[i].currnum - 1 != answer[i])
                {
                    return false;
                }
            }
            puzzleInputManager.ResetGuess();
            return true;
        }
        puzzleInputManager.ResetGuess();
        return false;
        
    }

}
