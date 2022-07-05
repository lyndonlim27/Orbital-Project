using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TrapData : SwitchData
{
    public string triggername;
    public bool ontrigger;
    public int damage;
    public Quaternion quaternion;
    public bool ranged;
    public bool horizontal;
    public bool flip;
    public List<RangedData> rangedDatas;
    
}
