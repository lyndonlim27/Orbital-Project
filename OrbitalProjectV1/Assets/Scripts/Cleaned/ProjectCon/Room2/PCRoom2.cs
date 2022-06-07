using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom2 : RoomManager
{
    private void Start()
    {
        roomtype = ROOMTYPE.FIGHTING_ROOM;
    }
    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        RoomChecker();

    }


}
