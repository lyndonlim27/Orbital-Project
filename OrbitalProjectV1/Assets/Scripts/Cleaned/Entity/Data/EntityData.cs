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
        BOSS,
        CAST_ONTARGET,
        CAST_SELF,
        PROJECTILE,
        CONSUMABLE_ITEM,
        TRAP,
        PUZZLEBOX,
        FODDER,
    }
    public enum PATTERN
    {
        SIMPLE_DIAG,
        //ZIG_ZAG,
        CROSS,
        //CIRCLE,
        BOX_LINE,
        PATTERN1,
        PATTERN2,
        PATTERN3,
        COUNT
    }
    public PATTERN pattern;
    public int condition;
    public TYPE _type;
    public bool multispawns;
    public Vector2 startPos;
    public Vector2 endPos;
    public bool random;
    public Vector2 pos;
    public GameObject damageFlicker;
    public bool spawnAtStart;
    public float scale;
    public bool staggered;
    public float staggerTime;
    public float duration;
    public Color defaultcolor;
}
