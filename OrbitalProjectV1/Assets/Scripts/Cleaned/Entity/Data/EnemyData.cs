using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ItemWithTextData
{
    public int damageValue;
    public float moveSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    public List<string> meleetriggers;
    public List<string> defends;
    public List<string> rangedtriggers;
    public string animatorname;
    public int words;
    public float rangedcooldown;
}
