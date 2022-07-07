using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom_Mgr : RoomManager
{

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
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
