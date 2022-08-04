using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.Behavioural
{
    public class DefeatedOnTrigger : MonoBehaviour
    {
        private Player player;
        private Collider2D col;

        private void Awake()
        {
            player = FindObjectOfType<Player>(true);
            col = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponentInChildren<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(player.GetAttackData().damage);
            }
        }
    }
}
