using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchRoom_Mgr : RoomManager
{
    LightSwitchSystem lightSwitchSystem;
    // Update is called once per frame
    Light2D[] candles;
    Animator[] animators;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.PUZZLE_ROOM;
        base.Awake();
        lightSwitchSystem = FindObjectOfType<LightSwitchSystem>(true);
        
    }

    protected void Start()
    {
        
    }


    protected override void Update()
    {
        base.Update();
        //if (activated)
        //{
        //    StartCoroutine(lightSwitchSystem.StartLightShow());
        //}

        Debug.Log(lightSwitchSystem.incoroutine);
        RoomChecker();
        CheckRunningEvents();

    }
   
}
