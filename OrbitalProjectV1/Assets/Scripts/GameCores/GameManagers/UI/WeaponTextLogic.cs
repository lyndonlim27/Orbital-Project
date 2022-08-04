using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace EntityCores
{
    public class WeaponTextLogic : TextLogic
    {
        private RoomManager roomManager;

        protected override bool CheckInternalInput()
        {
            return true;
        }

        protected override void GenerateNewWord()
        {
            remainingword = currentword;
        }

        protected override void PerformAction()
        {
            Destroy(parent.gameObject);
            this.player.GetComponent<WeaponPickup>().Swap(parent.GetData()._name);
        }

    }
}
