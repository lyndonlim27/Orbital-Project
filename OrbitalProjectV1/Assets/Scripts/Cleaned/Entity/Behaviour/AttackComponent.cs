using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackComponents handle all the forces;

public abstract class AttackComponent : MonoBehaviour
{
    
    protected DetectionScript detectionScript;
    protected EnemyBehaviour parent;
    protected Player target;
    protected EnemyData enemyData;
    protected PoolManager poolManager;

    private void Awake()
    {
        target = GameObject.FindObjectOfType<Player>();
        poolManager = FindObjectOfType<PoolManager>(true);
        
    }

    public virtual void Start()
    {
        //call our init function whenever Start is called;
        detectionScript = GetComponent<DetectionScript>();
        parent = transform.parent.GetComponent<EnemyBehaviour>();
        enemyData = parent.enemyData;    
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 1);
    }

    public bool detected()
    {
        return detectionScript.playerDetected;
    }
  
}
