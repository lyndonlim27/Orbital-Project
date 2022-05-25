using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackComponents handle all the forces;

public abstract class AttackComponent : MonoBehaviour
{
    
    protected DetectionScript detectionScript;
    protected EnemyBehaviour parent;
    protected EnemyData enemyData;
    protected Player target;
    public abstract void Attack();

    private void Awake()
    {
        target = GameObject.FindObjectOfType<Player>();
        
        
        
    }

    public virtual void Start()
    {
        //call our init function whenever Start is called;
        detectionScript = GetComponent<DetectionScript>();
        parent = transform.root.GetComponent<EnemyBehaviour>();
        enemyData = parent.enemyData;
        
        
        
    }

    public bool detected()
    {
        return detectionScript.playerDetected;
    }
  
}
