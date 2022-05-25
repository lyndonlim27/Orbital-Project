using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : EntityBehaviour
{
    [SerializeField] private EntityData itemData;

    public override void Defeated()
    {
        //drop some gold;
        Destroy(gameObject);
    }

    public override EntityData GetData()
    {
        return itemData;
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.itemData = stats;
    }
}
