using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellStats : ScriptableObject
{

    public enum Type
    {
        CAST,
        PROJECTILE,
    };
    public Type type;
    public int damage;
    public float speed, rotation, lifetime;
    public string trigger;
    public string ac_name;
 
}
