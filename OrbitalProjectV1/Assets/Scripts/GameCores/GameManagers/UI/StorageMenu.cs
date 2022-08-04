using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.UIComps
{
    public class StorageMenu : MenuBehaviour
    {

        protected override void Start()
        {
            base.Start();
            this.gameObject.SetActive(false);
        }
    }
}
