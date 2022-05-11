using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon8Direction : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    public Rigidbody2D rb;
    public Animator animator;
    public Bullet bulletPrefab;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);


        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        Quaternion angle = Quaternion.Euler(0, 0, Mathf.Atan2(movement.y, movement.x)
                * Mathf.Rad2Deg);
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, angle );
        bullet.Shoot(movement);
    }

}
