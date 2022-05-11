using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public PlayerMove player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab,shootingPoint.position,shootingPoint.rotation);
        bullet.transform.eulerAngles = player.transform.localPosition;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        rb.AddForce(player.movement * bulletForce, ForceMode2D.Impulse);
    }
}
