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
        CHEST
    }
    public ITEM_TYPE item_type;
    

    // Start is called before the first frame update

}
