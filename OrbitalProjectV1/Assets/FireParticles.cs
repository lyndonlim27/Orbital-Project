using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticles : MonoBehaviour
{
    ParticleSystem particles;
    // use enums to restrict the num of types;
    private enum TYPE
    {
        FIRE,
        WATER,
        EARTH,
        POISON,
        AIR,
    }
    private TYPE type;
    private int levels;
    private int damage;
    public Material currmaterial;
    private float rotation;
    private float speedMultiplier;
    private int maxparticles;
    [SerializeField] private Particle_Stats[] particle_stats;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        
    }


    private void Start()
    {
        Level1FB();
        LoadParticles();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Level3FB();
            LoadParticles();
        }

    }




    //FIRE PARTICLES maybe i will add into Scriptable objects, but i think no need.
    private void Level1FB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 360;
    }

    private void Level2FB()
    {
        damage = 1;
        maxparticles = 2;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 45;

    }

    private void Level3FB()
    {
        damage = 1;
        maxparticles = 5;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 360;

    }

    private void Level1WB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 360;
    }

    private void Level2WB()
    {
        damage = 1;
        maxparticles = 2;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 45;
    }

    private void Level3WB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.FIRE;
        rotation = 360;

    }

    private void Level1EB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.EARTH;
        rotation = 360;

    }

    private void Level2EB()
    {
        damage = 1;
        maxparticles = 2;
        speedMultiplier = 1;
        type = TYPE.EARTH;
        rotation = 45;

    }

    private void Level3EB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.EARTH;
        rotation = 360;

    }

    private void Level1PB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.POISON;

    }

    private void Level2PB()
    {
        damage = 1;
        maxparticles = 2;
        speedMultiplier = 1;
        type = TYPE.POISON;
        rotation = 45;

    }

    private void Level3PB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.POISON;
        rotation = 360;

    }

    private void Level1AB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.AIR;

    }

    private void LevelAFB()
    {
        damage = 1;
        maxparticles = 2;
        speedMultiplier = 1;
        type = TYPE.AIR;
        rotation = 45;

    }

    private void Level3AB()
    {
        damage = 1;
        maxparticles = 1;
        speedMultiplier = 1;
        type = TYPE.AIR;
        rotation = 360;


    }


    private void changeElement(TYPE type)
    {
        // can't just load like that, i still need the row and columns of the spritesheet.
        //Material mat = Resources.Load(string.Format("Material/{0}", nameof(type))) as Material;
        foreach (Particle_Stats particle in particle_stats)
        {
            if (particle.name == nameof(type))
            {
                var textureanim = particles.textureSheetAnimation;
                GetComponent<ParticleSystemRenderer>().material = particle.material;
                textureanim.numTilesX = particle.row;
                textureanim.numTilesY = particle.col;
            }
        }
    }

    private void LoadParticles()
    {
        //if particles not stopped, cant do any changes
        if (!particles.isStopped)
        {
            particles.Stop();
        }

        //change the attributes
        var mainx = particles.main;
        mainx.startSpeed = 5f;
        mainx.startSpeedMultiplier = speedMultiplier;
        mainx.maxParticles = maxparticles;
        var partem = particles.emission;
        partem.burstCount = maxparticles;
        changeElement(type);
        particles.Play();
    }
}
