using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.RoomManagers
{
    public class PressurePlateRoom_Mgr : RoomManager
    {


        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Update()
        {
            base.Update();
            PressurePlateCheck();
            RoomChecker();
            CheckRunningEvents();

        }

        protected override void RoomChecker()
        {
            if (pressureRoomComplete)
            {
                base.RoomChecker();
            }

        }
    }
}
