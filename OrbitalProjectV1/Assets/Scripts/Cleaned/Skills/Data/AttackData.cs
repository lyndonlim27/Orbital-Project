using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackData :SkillData
{
    public int numOfProjectiles;
    public int damage;

    public enum ATTACK_TYPE
    {
        FIRE,
        SHURIKEN
    }
    public ATTACK_TYPE attackType;
    public GameObject prefab;

}

