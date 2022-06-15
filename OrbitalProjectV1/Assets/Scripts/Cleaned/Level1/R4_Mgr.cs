using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R4_Mgr : RoomManager
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        roomtype = ROOMTYPE.HYBRID_ROOM;
        base.Awake();
        
    }
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
    }

}
