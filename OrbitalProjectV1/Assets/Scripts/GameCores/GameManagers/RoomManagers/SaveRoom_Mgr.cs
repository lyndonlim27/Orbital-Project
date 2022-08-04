using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.RoomManagers
{
    public class SaveRoom_Mgr : RoomManager
    {
        protected override void Awake()
        {
            this.roomtype = ROOMTYPE.SAVE_ROOM;
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
