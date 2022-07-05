using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemWithTextData : EntityData
{
    public ItemWithTextData[] itemTextDatas;
    public string ac_name;
    public string _trigger;
    public enum ITEM_TYPE
    {
        PUSHABLE,
        WEAPON,
        CHEST,
        OTHERS,
        TOMB,
        SAVEPOINT,
        PUZZLETORCH,
        BOSSPROPS
    }
    public ITEM_TYPE item_type;
    public Sprite secondarysprite;
    public string description;
    public RangedData rangedData;

    // Start is called before the first frame update

}
