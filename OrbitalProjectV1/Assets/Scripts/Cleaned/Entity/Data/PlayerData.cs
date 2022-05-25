using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : EntityData
{ 

    [Header("Player properties")]
    public int maxHealth;
    public int selfDamage;

    [Header("Movement")]
    public float _moveSpeed;
}
