using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.RoomManagers
{
    public class HangManRoom_Mgr : RoomManager
    {
        private bool playing;

        protected override void Awake()
        {
            base.Awake();
            playing = false;
        }

        protected override void Update()
        {
            base.Update();
            RoomChecker();
            if (!playing && activated)
            {
                playing = true;
                textDescription.StartDescription("You see a light at the end of the tunnel!");

            }



        }

    }
}
