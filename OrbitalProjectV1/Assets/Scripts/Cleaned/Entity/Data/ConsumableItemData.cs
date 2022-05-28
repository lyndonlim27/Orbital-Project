using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConsumableItemData : EntityData
{
    public int _gold;
    public int _health;
    public enum CONSUMABLE
    {
        HEALTH,
        GOLD
    }
    public CONSUMABLE _consumableType;
}
