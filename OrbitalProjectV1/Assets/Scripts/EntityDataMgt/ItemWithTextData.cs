using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDataMgt
{
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
            BOSSPROPS,
            BALL,
            PORTAL,
            LASER,
            MIRROR,
            MONSTERTRAPBOX
        }
        public ITEM_TYPE item_type;
        public Sprite secondarysprite;
        public Sprite[] itemSprites;
        public string description;
        public RangedData rangedData;

        // Start is called before the first frame update

    }
}
