using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParticleData : EntityData
{
    public int levels;
    public int damage;
    public Material material;

    [Range(0,360)]
    public float rotation;

    [Range(1,10)]
    public float speedMultiplier;

    [Range(1,30)]
    public int maxparticles;
    public bool activated;
    public bool loop;
    public int row;
    public int col;

}
