using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom3 : RoomManager
{
    public override void FulfillCondition(string key)
    {
        conditions.Remove(key);
    }

    public override void UnfulfillCondition(string key)
    {
        conditions.Add(key);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {

        base.Update();
        RoomChecker();
    }

    // Update is called once per frame

}
