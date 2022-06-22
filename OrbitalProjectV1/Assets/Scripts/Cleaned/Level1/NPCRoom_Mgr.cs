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
        roomtype = ROOMTYPE.PUZZLE_ROOM;
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
            //foreach(EntityData entitydata in _EntityData)
            //{
            //    if (entitydata._name == "key")
            //    {
            //        InstantiateEntity(entitydata);
            //        conditions.Add("key");
            //        added = true;
            //    }
            //}
           
        }
        
        base.RoomChecker();

    }

    protected void CheckNPCPrereq()
    {
        foreach (NPCBehaviour npc in npcs)
        {
            NPCData _data = npc.GetData() as NPCData;
            if (!conditions.Contains(_data.prereq._name + _data.prereq.GetInstanceID()))
            {
                npc.Proceed();
            }
        }
    }


    protected override void CheckRunningEvents()
    {
        if (dialMgr.playing || popUpSettings.gameObject.activeInHierarchy)
        {
            PauseGame();
        } else
        {
            ResumeGame();
        }

    }

}


