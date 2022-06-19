using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedOnTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if(collision.GetComponent<EnemyBehaviour>() != null)
        {
            collision.GetComponent<EnemyBehaviour>().Defeated();
        }
    }
}
