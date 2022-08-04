using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using PuzzleCores;

namespace GameManagement.RoomManagers
{
    public class LightSwitchRoom_Mgr : RoomManager
    {
        private LightSwitchSystem lightSwitchSystem;
        Light2D[] candles;
        Animator[] animators;

        protected override void Awake()
        {
            base.Awake();
            lightSwitchSystem = GetComponentInChildren<LightSwitchSystem>();

        }

        protected override void Update()
        {
            base.Update();
            RoomChecker();
            CheckRunningEvents();

        }

    }
}
