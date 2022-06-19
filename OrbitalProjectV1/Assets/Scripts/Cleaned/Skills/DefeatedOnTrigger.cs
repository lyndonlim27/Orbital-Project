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
        if(collision.GetComponentInChildren<EnemyBehaviour>() != null)
        {
            collision.GetComponentInChildren<EnemyBehaviour>().TakeDamage(player.GetAttackData().damage);
        }
    }
}
