using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1_Mgr : RoomManager
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
        //check condition 1

        if (activated && !conditions.Contains("booze"))
        {
            npcs[0].Proceed();
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
        Debug.Log(conditions.Count);
        base.RoomChecker();

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


