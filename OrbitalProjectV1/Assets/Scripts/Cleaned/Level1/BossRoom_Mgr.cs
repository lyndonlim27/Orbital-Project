using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom_Mgr : RoomManager
{
    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }
}
