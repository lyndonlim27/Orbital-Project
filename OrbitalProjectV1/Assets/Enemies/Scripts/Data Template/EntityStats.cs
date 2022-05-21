using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityStats : ScriptableObject
{
    public int words;
    public int damageValue;
    public float moveSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    public List<Spell> spells;
    public Sprite sprite;
    public string animatorname;
    
}
