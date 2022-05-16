using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{
    public List<GameObject> enemies;
    private GameObject GenerateObject()
    {
        return GameObject.Instantiate(enemies[Random.Range(0, enemies.Count)]);
    }
}
