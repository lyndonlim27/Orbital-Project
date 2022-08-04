using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.Behavioural
{
    [RequireComponent(typeof(Collider2D))]
    public class DetectionScript : MonoBehaviour
    {
        public bool playerDetected;
        private Collider2D col;
        private EnemyBehaviour enemy;
        public bool _dialoguePlayed;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            playerDetected = false;
            enemy = GetComponentInParent<EnemyBehaviour>();
        }

        private void OnEnable()
        {
            playerDetected = false;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerDetected = true;
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerDetected = false;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerDetected = true;
            }
            else if (collision.gameObject.CompareTag("Stealth"))
            {
                playerDetected = false;
            }
        }

        
    }
}
