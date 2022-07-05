using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffData : SkillData
{ 
    public float speedAmount;
    public int healAmount;
    public enum BUFF_TYPE
    {
        SPEED,
        HEAL,
        STEALTH,
        INVULNERABLE
    }

    public BUFF_TYPE buffType;
}
