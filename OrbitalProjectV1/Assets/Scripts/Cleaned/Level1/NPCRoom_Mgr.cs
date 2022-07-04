using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRoom_Mgr : RoomManager
{
    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }

    protected override void RoomChecker()
    {
        if (activated)
        {
            CheckNPCPrereq();
        }
        
        base.RoomChecker();

    }

}


