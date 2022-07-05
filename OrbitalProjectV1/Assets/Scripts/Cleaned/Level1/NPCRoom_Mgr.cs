using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRoom_Mgr : RoomManager
{
    private bool added;
    protected override void Awake()
    {
        base.Awake();
        //door = GameObject.FindObjectOfType<UnlockableDoor>();

    }

    private void Start()
    {
        //added = false;
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }

    protected override void RoomChecker()
    {
        //check condition 1

        if (activated)
        {
            CheckNPCPrereq();

           
        }
        
        base.RoomChecker();

    }



}


