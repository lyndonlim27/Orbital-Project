using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R3_Mgr : RoomManager
{

    protected override void Awake()
    {
        roomtype = ROOMTYPE.TREASURE_ROOM;
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(CanProceed());
        RoomChecker();
        CheckRunningEvents();
    }

}
