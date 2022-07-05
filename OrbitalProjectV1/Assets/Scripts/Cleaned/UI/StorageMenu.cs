using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageMenu : MenuBehaviour
{

    protected override void Start()
    {
        base.Start();
        this.gameObject.SetActive(false);
    }
}
