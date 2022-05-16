using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 _movement;
    private Rigidbody2D _rb;
    public Animator _animator;
    private Transform _target;
    private int _currHealth;
    private Weapon _currWeapon;
    private WeaponPickup _weaponManager;
    private SpriteRenderer _spriteRenderer;
    private DamageFlicker _flicker;
    private float _time = 0;
    private float _timeDelay = 0;

    [Header("Player properties")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int selfDamage = 10;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;


    [Header("Player UI")]
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private HealthBar healthBar;

    int count = 0;


    // Start is called before the first frame update
    void Start()
    {
        _currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        _weaponManager = this.gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponPickup>();
        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flicker = GetComponent<DamageFlicker>();
    }

    // Update is called once per frame
    void Update()
    {
        _time = _time + 1f * Time.deltaTime;
        if(_currHealth == 0)
        {
            Death();
        }
   

        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();

        if (Input.GetButtonDown("Shoot") || Input.GetMouseButtonDown(0))
        {
            Shoot();
            _timeDelay = 0.5f;
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currHealth > 0)
            {
                TakeDamage(selfDamage);
            }
        }
       // if (count < 1)
       if(_time >= _timeDelay)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");


            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetFloat("Vertical", _movement.y);
            _animator.SetFloat("Speed", _movement.magnitude);
            _currWeapon.TurnWeapon(_movement);
            _time = 0;
            _timeDelay = 0;
        }
       // count--;


    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }


    private void TakeDamage(int damageTaken)
    {
        _currHealth -= damageTaken;
        healthBar.SetHealth(_currHealth);
        _flicker.Flicker();
    }

    private void Shoot()
    {
        count = 50;
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _currWeapon.Shoot(point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
    }
    
    public void PickupItem(string weapon)
    {   
        _weaponManager.Swap(weapon);
    }

    IEnumerator FadeOut()
    {
        for(float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = _spriteRenderer.material.color;
            c.a = f;
            _spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        _gameOver.Setup();
        this.gameObject.SetActive(false);
    }

    private void Death()
    {
        StartCoroutine("FadeOut");
    }
}