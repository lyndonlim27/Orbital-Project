using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleCores;

namespace GameManagement.RoomManagers
{
    public class TorchPuzzleRoom_Mgr : RoomManager
    {
        private TorchPuzzle torchPuzzle;
        protected override void Awake()
        {
            base.Awake();
            torchPuzzle = GetComponentInChildren<TorchPuzzle>();
            torchPuzzle.SetCurrentRoom(this);
        }

        protected override void Update()
        {

            base.Update();
            if (activated && !torchPuzzle.activated)
            {
                torchPuzzle.GetTorches();
                torchPuzzle.ActivatePuzzle(1);

            }
            RoomChecker();
            CheckRunningEvents();

        }

        protected override bool CanProceed()
        {
            return torchPuzzle.isComplete;
        }
    }
}
