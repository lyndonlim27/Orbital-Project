using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzleRoom_Mgr : RoomManager
{
    TorchPuzzle torchPuzzle;
    protected override void Awake()
    {
        base.Awake();
        torchPuzzle = FindObjectOfType<TorchPuzzle>(true);
    }

    protected override void Update()
    {

        base.Update();
        if (activated && !torchPuzzle.activated)
        {
            torchPuzzle.ActivatePuzzle();
        }
        RoomChecker();
        CheckRunningEvents();

    }

    protected override bool CanProceed()
    {
        return torchPuzzle.isComplete;
    }
}
