using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    private Vector2 moveDirection;
    public InputAction playerControls;
    private Rigidbody2D _rb;
    [SerializeField] private int moveSpeed;
    // Start is called before the first frame update

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(moveDirection.x * moveSpeed + 6, moveDirection.y * moveSpeed);
    }


}
