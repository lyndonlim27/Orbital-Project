using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : EntityData
{
    public int damageValue;
    public float moveSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    public List<RangedBehaviour> rangeds;
    public string animatorname;
    public int words;
}
