using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : EntityBehaviour
{
    PlayerData playerData;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Collider2D col;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private DamageFlicker _flicker;
    private float _time = 0;
    private float _timeDelay = 0;
    private DialogueManager dialMgr;
    private int _currHealth;
    private Weapon _currWeapon;
    private WeaponPickup _weaponManager;



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
        _currHealth = playerData.maxHealth;
        healthBar.SetMaxHealth(playerData.maxHealth);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weaponManager = this.gameObject.GetComponentInChildren<WeaponPickup>();
        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flicker = GetComponent<DamageFlicker>();
        dialMgr = GameObject.FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //freeze player's actions when inside dialogue
        if (dialMgr.playing)
        {
            return;
        }

        _time += 1f * Time.deltaTime;
        if (_currHealth <= 0)
        {
            Defeated();
        }

        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();

        CheckCombat();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currHealth > 0)
            {
                TakeDamage(3);
                _rb.AddForce(transform.forward * 15000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
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
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
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
        if (dialMgr.playing)
        {
            return;
        }

        
        transform.position = (Vector2) transform.position +_movement.normalized * playerData._moveSpeed * Time.fixedDeltaTime;
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
    public void Shoot(EntityBehaviour entity)
    {
        _timeDelay = 0.5f;
        Debug.Log("Shoot");
        //Debug.Log(enemy);
        Vector2 point2Target = (Vector2)transform.position - (Vector2)entity.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _currWeapon.Shoot(entity, point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
    }

       
    public void PickupItem(string weapon)
    {   
        _weaponManager.Swap(weapon);
    }

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
    public override void Defeated()
    {
        StartCoroutine("FadeOut");
    }

}