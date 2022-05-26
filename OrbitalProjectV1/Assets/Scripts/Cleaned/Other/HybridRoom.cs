using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridRoom : RoomManager
{
    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Update()
    {
        
    }

    protected override void RoomChecker()
    {
       
        //check condition 2 and onwards;
        if (!conditions.Contains("key"))
        {
            //unlock the other door
        }
    }

    public override void FulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }

    protected override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }
}
