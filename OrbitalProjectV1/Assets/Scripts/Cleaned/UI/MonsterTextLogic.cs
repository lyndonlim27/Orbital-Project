using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextLogic : TextLogic{
    // Start is called before the first frame update
    protected override void Awake()
    {

        base.Awake();
        minDist = 3f;

    }
}
