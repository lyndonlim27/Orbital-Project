using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillData : ScriptableObject
{
    public enum SKILLTYPE
    {
        DEBUFF,
        BUFF,
        ATTACK
    }
    public SKILLTYPE skillType;
    public float cooldown;
    public float duration;
    public int manaCost;
    public Sprite sprite;
    public string skillName;
    public int goldCost;
}
