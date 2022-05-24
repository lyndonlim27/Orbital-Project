using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : EntityData
{
    public int damageValue;
    public float moveSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    public List<Spell> spells;
    public string animatorname;
}
