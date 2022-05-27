using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : RoomManager
{
    protected override void Awake()
    {
        base.Awake();

    }

    public override void FulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }

    protected override void RoomChecker()
    {
        throw new System.NotImplementedException();
    }

    public override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }
}
