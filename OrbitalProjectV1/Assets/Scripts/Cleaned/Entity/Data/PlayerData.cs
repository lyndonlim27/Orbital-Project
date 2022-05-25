using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PlayerData : EntityData
{ 

    [Header("Player properties")]
    public int maxHealth;
    public int selfDamage;

    [Header("Movement")]
    public float _moveSpeed;
}
