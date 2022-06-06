using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_Mgr : RoomManager
{
    private bool added;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.FIGHTING_ROOM;
        base.Awake();

    }

    private void Start()
    {
        added = false;
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }

    protected override void RoomChecker()
    {
        base.RoomChecker();

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
