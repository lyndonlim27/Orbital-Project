using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.RoomManagers
{
    public class FightRoom_Mgr : RoomManager
    {
        public int waveNum;

        protected override void Start()
        {
            base.Start();
            startNum = 1;
        }


        protected override void Update()
        {
            base.Update();
            FightRoomCheck(waveNum);
            RoomChecker();
            CheckRunningEvents();

        }



        protected override bool CanProceed()
        {
            return activated && startNum >= waveNum && CheckEnemiesDead();
        }

    }
}
