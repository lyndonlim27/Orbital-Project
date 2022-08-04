using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace EntityCores
{
    public class ItemTextLogic : TextLogic
    {
        private RoomManager roomManager;

        protected override void Start()
        {
            base.Start();

        }

        protected override void OnEnable()
        {
            base.OnEnable();

        }
        protected override bool CheckInternalInput()
        {
            return true;
        }

        public void ResetWord()
        {
            GenerateNewWord();
        }

        protected override void GenerateNewWord()
        {
            currentword = parent.GetData()._name.ToLower();
            this.remainingword = parent.GetData()._name.ToLower();
        }

        protected override void PerformAction()
        {
            parent.GetComponent<EntityBehaviour>().Defeated();
        }
    }
}
