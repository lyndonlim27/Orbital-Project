using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public string _name;
    public float minDist;
    public bool isStateful;
    public Sprite sprite;
    public int condition;
    public string placementType;
    public Vector2 pos;
    public int gold;
    private DamageFlicker _flicker;
}
