using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedOnTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        EnemyBehaviour enemy = collision.gameObject.GetComponentInChildren<EnemyBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(player.GetAttackData().damage);
        }
    }
}
