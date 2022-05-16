using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public GameObject parent;
    protected float attackRange;
    protected float attackSpeed;
    protected int damageValue;
    [SerializeField]
    private EntityStats enemyStats;
    public abstract void Attack(Player target);

    private void Start()
    {
        //call our init function whenever Start is called;
        Init();
    }

    public virtual void Init()
    {
        attackRange = enemyStats.attackRange;
        attackSpeed = enemyStats.attackSpeed;
        damageValue = enemyStats.damageValue;
    }

}
