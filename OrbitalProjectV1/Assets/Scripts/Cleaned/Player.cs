using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : EntityBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Collider2D col;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float _time = 0;
    private float _timeDelay = 0;
    private DialogueManager dialMgr;
    private int _currHealth;
    public int currMana { get; private set;}
    private Weapon _currWeapon;
    private WeaponPickup _weaponManager;
    private DamageFlicker _flicker;
    private GoldCounter _goldCounter;
    public int currGold { get; private set;}


    [Header("Player UI")]
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ManaBar _manaBar;
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
        currMana = playerData.maxMana;
        currGold = playerData.gold;
        _healthBar.SetMaxHealth(playerData.maxHealth);
        _manaBar.SetMaxMana(playerData.maxMana);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weaponManager = this.gameObject.GetComponentInChildren<WeaponPickup>();
        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        dialMgr = GameObject.FindObjectOfType<DialogueManager>();
        _goldCounter = FindObjectOfType<GoldCounter>(true);
        _goldCounter.GoldUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        _time += 1f * Time.deltaTime;
        if (_currHealth <= 0)
        {
            Defeated();
        }

        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();

        CheckCombat();

        if (_currHealth > 100)
        {
            _currHealth = 100;
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
        /*
        if (dialMgr.playing)
        {
            return;
        }*/

        
        transform.position = (Vector2) transform.position +_movement.normalized * playerData._moveSpeed * Time.fixedDeltaTime;
        //_rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }


    //When player takes damage, reduce current health and flicker sprite
    public void TakeDamage(int damageTaken)
    {
        _currHealth -= damageTaken;
        _healthBar.SetHealth(_currHealth);
        _flicker.Flicker(this);
        if (_currHealth <= 0)
        {
            Defeated();
        }
    }

    public void AddHealth(int health)
    {
      
        _currHealth = Math.Min(_currHealth+health,100);
        _healthBar.SetHealth(_currHealth);
        
    }

    public void AddGold(int gold)
    {
        currGold += gold;
        _goldCounter.GoldUpdate();
    }

    public void UseGold(int gold)
    {
        currGold -= gold;
        _goldCounter.GoldUpdate();
    }

    //When player shoot, player direction faces the target enemy
    public void Shoot(EntityBehaviour entity)
    {
        _timeDelay = 0.5f;
        //Debug.Log(enemy);
        Vector2 point2Target = (Vector2)transform.position - (Vector2)entity.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _currWeapon.Shoot(entity, point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
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

    public override void SetEntityStats(EntityData stats)
    {
        this.playerData = (PlayerData) stats;
    }

    public override EntityData GetData()
    {
        return playerData;
    }


    public void UseMana(int manaCost)
    {
        currMana -= manaCost;
        _manaBar.SetMana(currMana);
    }

    public void AddMana(int mana)
    {
        currMana += mana;
        _manaBar.SetMana(currMana);
    }
}