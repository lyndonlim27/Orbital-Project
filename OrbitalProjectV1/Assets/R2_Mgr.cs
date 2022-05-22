using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_Mgr : RoomManager
{
    protected override void Awake()
    {
        base.Awake();
        this.maxEnemies = 0;
        
    }

    protected override void InitializeRoom()
    {
        base.InitializeRoom();
        //maybe wanna do other stuffs; 
    }
}
