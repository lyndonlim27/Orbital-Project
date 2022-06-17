using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PlayerData : EntityData
{ 

    [Header("Player properties")]
    public int maxHealth;
    public int maxMana;
    public int gold;
    public bool ranged;
    public DebuffData debuffData;
    public BuffData buffData;
    public AttackData attackData;

    [Header("Movement")]
    public float _moveSpeed;
}
