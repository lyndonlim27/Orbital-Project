using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R5_Mgr : RoomManager
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    protected override void Update()
    {
        base.Update();
        RoomChecker();
    }
}
