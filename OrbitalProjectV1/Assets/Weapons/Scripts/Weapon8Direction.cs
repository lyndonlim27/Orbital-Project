using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon8Direction : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float MovementSpeed;
    private Vector2 MovementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Animate();
    }

    private void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        if (Horizontal == 0 && Vertical == 0)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }

        MovementInput = new Vector2(Horizontal, Vertical);
        rb.velocity = MovementInput * MovementSpeed * Time.deltaTime;
    }

    private void Animate()
    {
        anim.SetFloat("MovementX", MovementInput.x);
        anim.SetFloat("MovementY", MovementInput.y);
    }
}
