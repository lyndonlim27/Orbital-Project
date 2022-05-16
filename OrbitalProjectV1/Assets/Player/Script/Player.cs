using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Collider2D col;
    public Animator _animator;
    private Transform _target;
    private int currHealth;

    [Header("Player properties")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int selfDamage = 10;
    private Weapon currWeapon;
    private WeaponPickup weaponManager;


    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    int count = 0;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        
    }

    public bool isDead()
    {
        Debug.Log(currHealth);
        return this.currHealth <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        weaponManager = this.gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponPickup>();
        currWeapon = weaponManager.ActiveWeapon().GetComponent<Weapon>();

    }

    // Update is called once per frame
    void Update()
    {   /*
        if(currHealth == 0)
        {
            Death();
        }
        */

        currWeapon = weaponManager.ActiveWeapon().GetComponent<Weapon>();

        if (Input.GetButtonDown("Shoot") || Input.GetMouseButtonDown(0))
        {
            Shoot();
            
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(selfDamage);
        }
        if (count < 1)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");


            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetFloat("Vertical", _movement.y);
            _animator.SetFloat("Speed", _movement.magnitude);
            currWeapon.TurnWeapon(_movement);
        }
        count--;


    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }


    public void TakeDamage(int damageTaken)
    {
        currHealth -= damageTaken;
        healthBar.SetHealth(currHealth);
    }

    private void Shoot()
    {
        count = 50;
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        currWeapon.Shoot(point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
    }
    
    public void PickupItem(Weapon weapon)
    {   
        weaponManager.Swap(weapon);
    }
    


    private void Death()
    {
    }
}