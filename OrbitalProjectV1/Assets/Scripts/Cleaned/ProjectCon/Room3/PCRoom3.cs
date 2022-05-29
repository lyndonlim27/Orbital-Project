using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom3 : RoomManager
{
    int cooldown;
    //ConsumableItemData consumables;

    public PolygonCollider2D consumableSpawn;

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
        cooldown = 600;
    }

    protected override void Update()
    {

        if (cooldown == 0)
        {
            
        }

        base.Update();
        RoomChecker();
        
    }

    // Update is called once per frame

}
