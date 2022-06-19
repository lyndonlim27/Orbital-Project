using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R16_Mgr : RoomManager
{
    int waveNum;

    private void Start()
    {
        waveNum = Random.Range(4, 7);
    }


    protected override void Update()
    {
        base.Update();
        Debug.Log(CheckEnemiesDead());
        if (CheckEnemiesDead() && waveNum != 0)
        {
            int rand = Random.Range(3, 5);

            for (int i = 0; i < rand; i++)
            {
                EntityData entitydata = _EntityDatas[Random.Range(0, _EntityDatas.Length)];
                SpawnObject(entitydata);
            }
            waveNum--;

        }
        RoomChecker();
        CheckRunningEvents();

    }



    protected override bool CanProceed()
    {
        return waveNum == 0 && CheckEnemiesDead();
    }
}
