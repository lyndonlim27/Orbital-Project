using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 movement;
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField]
    private float health = 100;
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

        //moveplayer here using transform ohh i forgot your current position yes try run
        transform.position += new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(new Vector2(300, 0));
    }

    private void FixedUpdate()
    {

        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        this.health -= damage;
        
    }
}