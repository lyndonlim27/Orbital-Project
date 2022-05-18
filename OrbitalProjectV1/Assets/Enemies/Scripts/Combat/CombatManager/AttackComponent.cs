using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackComponents handle all the forces;

public abstract class AttackComponent : MonoBehaviour
{
    
    protected DetectionScript detectionScript;
    protected Entity parent;
    protected EntityStats enemyStats;
    protected Player target;
    public abstract void Attack();

    public virtual void Start()
    {
        //call our init function whenever Start is called;
        detectionScript = GetComponent<DetectionScript>();
        parent = transform.root.GetComponent<Entity>();
        GameObject go = GameObject.FindWithTag("Player");
        if (go != null)
        {
            target = go.GetComponent<Player>();
        }
        enemyStats = parent.stats;
    }

    public bool detected()
    {
        return detectionScript.playerDetected;
    }
  
}
