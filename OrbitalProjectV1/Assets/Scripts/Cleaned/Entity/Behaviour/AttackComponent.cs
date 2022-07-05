using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackComponents handle all the forces;

public abstract class AttackComponent : MonoBehaviour
{
    
    protected DetectionScript detectionScript;
    public EnemyBehaviour parent { get; protected set; }
    protected Player target;
    protected EnemyData enemyData;
    protected PoolManager poolManager;

    protected virtual void Awake()
    {
        target = GameObject.FindObjectOfType<Player>();
        poolManager = FindObjectOfType<PoolManager>(true);
        detectionScript = GetComponent<DetectionScript>();
        parent = transform.parent.GetComponent<EnemyBehaviour>();
        if (parent != null)
        {
            enemyData = parent.enemyData;
        }

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
