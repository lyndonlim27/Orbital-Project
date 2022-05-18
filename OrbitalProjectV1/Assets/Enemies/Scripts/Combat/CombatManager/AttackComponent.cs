using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackComponents handle all the forces;

public abstract class AttackComponent : MonoBehaviour
{
    
    protected DetectionScript detectionScript;
    protected Entity parent;
    protected EntityStats enemyStats;
    protected Player target = null;
    public abstract void Attack();

    public virtual void Start()
    {
        //call our init function whenever Start is called;
        detectionScript = GetComponent<DetectionScript>();
        parent = transform.root.GetComponent<Entity>();
        enemyStats = parent.stats;
    }

    public void Update()
    {
        if (detectionScript.playerDetected != null)
        {
            target = detectionScript.playerDetected.GetComponent<Player>();
        } else
        {
            target = null;
        }

        
    }

    public Player GetPlayer()
    {
        return target;
    }
  
}
