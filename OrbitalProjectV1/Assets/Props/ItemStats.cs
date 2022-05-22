using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
{
    public string _name;
    public int _count;
    public float mindist;
    public float speed;
    public string dir;
    public int damage;
    public string type;
    public Sprite sprite;
    public int conditional;
}
