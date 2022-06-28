using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchRoom_Mgr : RoomManager
{
    private LightSwitchSystem lightSwitchSystem;
    Light2D[] candles;
    Animator[] animators;

    protected override void Awake()
    {
        base.Awake();
        lightSwitchSystem = FindObjectOfType<LightSwitchSystem>(true);
        
    }

    protected void Start()
    {
        
    }


    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();

    }
   
}
