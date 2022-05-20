using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Collider2D col;
    public Animator _animator;
    private Transform _target = null;
    private int _currHealth;
    private Weapon _currWeapon;
    private WeaponPickup _weaponManager;
    private SpriteRenderer _spriteRenderer;
    private DamageFlicker _flicker;
    private float _time = 0;
    private float _timeDelay = 0;

    [Header("Player properties")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int selfDamage = 100;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    
    [Header("Player UI")]
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private HealthBar healthBar;
    public bool inCombat { get; private set; }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        inCombat = false;
        
    }

    public bool isDead()
    {
        return this._currHealth <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        GameObject _gameObj = GameObject.FindGameObjectWithTag("Enemy");
        if(_gameObj != null)
        {
            _target = _gameObj.transform;
        }
        _weaponManager = this.gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponPickup>();
        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flicker = GetComponent<DamageFlicker>();
    }

    // Update is called once per frame
    void Update()
    {
        //freeze player's actions when inside dialogue
        if (DialogueManager.GetInstance().playing)
        {
            return;
        }

        _time += 1f * Time.deltaTime;
        if (_currHealth <= 0)
        {
            Death();
        }

        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();

        //if (Input.GetButtonDown("Shoot") || Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
        //    _timeDelay = 0.5f; //When player shoots, pauses direction for 0.5 seconds

        //}
        CheckCombat();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currHealth > 0)
            {
                TakeDamage(selfDamage);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            UnlockableDoor door = GameObject.FindObjectOfType<UnlockableDoor>();
            Debug.Log(door);
            if (door != null)
            {
                door.UnlockDoor();
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            UnlockableDoor door = GameObject.FindObjectOfType<UnlockableDoor>();
            Debug.Log(door);
            if (door != null)
            {
                door.LockDoor();
            }
        }

        if (_time >= _timeDelay)
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

    }

    private void CheckCombat()
    {
        for (int i = (int)KeyCode.A; i < (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                inCombat = true;
            }
            else
            {
                inCombat = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //Move player's position
        //freeze player's movement when inside dialogue
        if (DialogueManager.GetInstance().playing)
        {
            return;
        }
        transform.position = (Vector2) transform.position +_movement.normalized * _moveSpeed * Time.fixedDeltaTime;
        //_rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }


    //When player takes damage, reduce current health and flicker sprite
    public void TakeDamage(int damageTaken)
    {
        _currHealth -= damageTaken;
        healthBar.SetHealth(_currHealth);
        _flicker.Flicker();
    }

    //When player shoot, player direction faces the target enemy
    public void Shoot(Entity enemy)
    {
        _timeDelay = 0.5f;
        Debug.Log("Shoot");
        //Debug.Log(enemy);
        Vector2 point2Target = (Vector2)transform.position - (Vector2)enemy.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _currWeapon.Shoot(enemy, point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
    }

       
    public void PickupItem(string weapon)
    {   
        _weaponManager.Swap(weapon);
    }

    //public bool OutOfRange(Entity entity)
    //{
    //    return
    //        //check if we are in range.
    //        Vector2.Distance(transform.position, entity.transform.position) <= _currWeapon.range &&
    //        //check for line of sight
    //        Physics2D.Linecast(transform.position,entity.transform.position).collider.gameObject.tag != "Enemy";
    //}

    //Fades sprite
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

    //When current health reaches 0, character dies and fade out
    private void Death()
    {
        StartCoroutine("FadeOut");
    }

}