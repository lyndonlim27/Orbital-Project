using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextDisplayer : TextDisplayer
{
    // Start is called before the first frame update
    protected override void Awake()
    {

        base.Awake();
        minDist = 3f;

    }
}
