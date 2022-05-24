using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    ParticleSystem particles;
    ParticleData particleData;

    /**
     * Retrieving Data.
     */
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        
    }

    /**
     * Initialising gameObject.
     */
    private void Start()
    {
        LoadParticles();
        
    }

    /**
     * Change the Material of the particles.
     */
    private void ChangeMaterial()
    {
        var textureanim = particles.textureSheetAnimation;
        GetComponent<ParticleSystemRenderer>().material = particleData.material;
        textureanim.numTilesX = particleData.row;
        textureanim.numTilesY = particleData.col;
           
        
    }

    /**
     * Change the Emission of the particles.
     */
    private void ChangeParticleEmission()
    {
        var partem = particles.emission;
        partem.burstCount = particleData.maxparticles;
    }

    /**
     * Change the Physical Stats of the particles.
     */
    private void ChangeParticleMain()
    {
        var mainx = particles.main;
        mainx.startSpeed = 5f;
        mainx.startSpeedMultiplier = particleData.speedMultiplier;
        mainx.maxParticles = particleData.maxparticles;
    }

    /**
     * Load the changes to the particle system.
     */
    private void LoadParticles()
    {
        //if particles not stopped, cant do any changes
        if (!particles.isStopped)
        {
            particles.Stop();
        }

        //change the attributes
        ChangeParticleMain();
        ChangeParticleEmission();
        particles.Play();
    }

    
}
