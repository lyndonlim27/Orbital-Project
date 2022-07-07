using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R23_Mgr : RoomManager
{
    protected override void Awake()
    {
        roomtype = ROOMTYPE.TREASURE_ROOM;
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }


}
