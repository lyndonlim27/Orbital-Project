using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightRoom_Mgr : RoomManager 
{
    int waveNum;
    int startNum;

    private void Start()
    {
        //waveNum = Random.Range(4, 7);
        waveNum = 1;
        startNum = 1;
    }


    protected override void Update()
    {
        base.Update();
        Debug.Log(CheckEnemiesDead());
        if (CheckEnemiesDead() && startNum != waveNum)
        {
            int rand = Random.Range(3, 5);
            startNum++;
            textDescription.StartDescription("Wave " + startNum);
            for (int i= 0; i < rand; i++)
            {
                EntityData entitydata = _EntityDatas[Random.Range(0, _EntityDatas.Length)];
                SpawnObject(entitydata);
            }
        }
        RoomChecker();
        CheckRunningEvents();
        
    }



    protected override bool CanProceed()
    {
        return activated && startNum >= waveNum && CheckEnemiesDead();
    }



}
