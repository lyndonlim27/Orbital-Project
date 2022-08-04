using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.RoomManagers
{
    public class TreasureRoom_Mgr : RoomManager
    {

        protected override void Awake()
        {
            base.Awake();
        }


        protected override void Update()
        {
            base.Update();

            RoomChecker();
            CheckRunningEvents();
        }

    }
}
