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
    public float maxDist;
    public List<string> meleetriggers;
    public List<string> defends;
    public List<string> rangedtriggers;
    public List<string> dashattacks;
    public List<RangedData> rangedDatas;
    public List<TrapData> bossdamageprops;
    public List<EnemyData> bossSummonprops;
    //public List<EnemyBehaviour> bossSummons;
    public List<ItemWithTextData> bossdefenceprops;
    public string animatorname;
    public int words;
    public float rangedcooldown;
    public bool stage2;
    public bool healStage;
    public Color enragedColor;
}
