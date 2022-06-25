using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateRoom_Mgr : RoomManager
{   
    
    private bool spawned;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.FIGHTING_ROOM;
        base.Awake();

    }

    protected void Start()
    {
        spawned = false;
        
    }

    protected override void Update()
    {
        base.Update();
        PressurePlateCheck();
        RoomChecker();
        CheckRunningEvents();

    }


    protected override void CheckRunningEvents()
    {
        if (dialMgr.playing || popUpSettings.gameObject.activeInHierarchy)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

    }

}
