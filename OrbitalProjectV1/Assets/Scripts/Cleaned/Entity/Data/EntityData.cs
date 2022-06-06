using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public string _name;
    public float minDist;
    public Sprite sprite;
    public enum TYPE
    {
        PRESSURE_SWITCH,
        SWITCH,
        OBJECT,
        NPC,
        ITEM,
        ENEMY,
        BOSSPROPS,
        BOSS
    }
    public int condition;
    public TYPE _type;
    public bool random;
    public Vector2 pos;
    public GameObject damageFlicker;
    public bool spawnAtStart;
    public float scale;
}
