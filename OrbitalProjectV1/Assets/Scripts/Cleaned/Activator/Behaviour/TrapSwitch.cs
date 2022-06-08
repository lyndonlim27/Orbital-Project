using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSwitch : ActivatorBehaviour
{

    RangedBehaviour ranged;
    List<RangedData> rangedDatas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetEntityStats(EntityData stats)
    {
        throw new System.NotImplementedException();
    }

    public override void Defeated()
    {
        throw new System.NotImplementedException();
    }

    public override EntityData GetData()
    {
        throw new System.NotImplementedException();
    }
}
