using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightRoom_Mgr : RoomManager 
{
    [SerializeField] private int waveNum;
    
    private void Start()
    {
        startNum = 1;
    }


    protected override void Update()
    {
        base.Update();
        FightRoomCheck(waveNum);
        RoomChecker();
        CheckRunningEvents();

    }

    

    protected override bool CanProceed()
    {
        return activated && startNum >= waveNum && CheckEnemiesDead();
    }



}
