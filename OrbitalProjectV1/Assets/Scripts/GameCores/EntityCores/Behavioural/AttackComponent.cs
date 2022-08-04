using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDataMgt;
using GameManagement;

// AttackComponents handle all the forces;
namespace EntityCores.Behavioural
{
    public abstract class AttackComponent : MonoBehaviour
    {

        protected DetectionScript detectionScript;
        public EnemyBehaviour parent { get; protected set; }
        protected Player target;
        protected EnemyData enemyData;
        protected PoolManager poolManager;

        protected virtual void Awake()
        {

            poolManager = FindObjectOfType<PoolManager>(true);
            target = FindObjectOfType<Player>(true);
            detectionScript = GetComponent<DetectionScript>();
            parent = transform.parent.GetComponent<EnemyBehaviour>();
            if (parent != null)
            {
                enemyData = parent.enemyData;
            }

        }
        protected void Start()
        {
            poolManager = PoolManager.instance;
            target = Player.instance;
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
}
