using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : EntityBehaviour, ISaveable
{

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
    private BuffBehaviour _buffBehaviour;
    private bool _invulnerable;


    [Header("Player Data")]
    [SerializeField] private int maxMana;
    [SerializeField] private int maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool ranged;
    [SerializeField] private BuffData _buffData;
    [SerializeField] private DebuffData _debuffData;
    [SerializeField] private AttackData _attackData;


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
        currGold = 100;
        currMana = maxMana;
        _currHealth = maxHealth;
        _healthBar.SetMaxHealth(maxHealth);
        _manaBar.SetMaxMana(maxMana);
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weaponManager = this.gameObject.GetComponentInChildren<WeaponPickup>();
        _currWeapon = _weaponManager.ActiveWeapon().GetComponent<Weapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flicker = GameObject.FindObjectOfType<DamageFlicker>();
        dialMgr = GameObject.FindObjectOfType<DialogueManager>();
        _goldCounter = FindObjectOfType<GoldCounter>(true);
        _goldCounter.GoldUpdate();
        _buffBehaviour = FindObjectOfType<BuffBehaviour>(true);
        _invulnerable = false;
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

        
        //transform.position = (Vector2) transform.position +_movement.normalized * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }


    //When player takes damage, reduce current health and flicker sprite
    public void TakeDamage(int damageTaken)
    {
        Debug.Log(_invulnerable);
        if (!_invulnerable)
        {
            _currHealth -= damageTaken;
            _healthBar.SetHealth(_currHealth);
            _flicker.Flicker(this);
            if (_currHealth <= 0)
            {
                Defeated();
            }
        }
    }

    public void AddHealth(int health)
    {
      if(_currHealth + health > maxHealth)
        {
            _currHealth = maxHealth;
        }
        else
        {
            _currHealth = Math.Min(_currHealth + health, 100);
        }
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

    public void UseMana(int manaCost)
    {
        currMana -= manaCost;
        _manaBar.SetMana(currMana);
    }

    public void AddMana(int mana)
    {
        if(currMana + mana > maxMana)
        {
            currMana = maxMana;
        }
        else
        {
            currMana += mana;
        }
        _manaBar.SetMana(currMana);
    }

    //When player shoot, player direction faces the target enemy
    public void Shoot(EntityBehaviour entity)
    {
        if (_buffBehaviour.inStealth)
        {
            StartCoroutine(_buffBehaviour.Unstealth());
        }
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
    }

    public override EntityData GetData()
    {
        throw new NotImplementedException();
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed += speed;
    }


    public RoomManager GetCurrentRoom()
    {
        return currentRoom;
    }

    public void SetInvulnerability(bool state)
    {
        _invulnerable = state;
    }

    public Vector2 GetDirection()
    {
        return _movement;
    }

    /*
     * Getters
     */

    public DebuffData GetDebuffData()
    {
        return _debuffData;
    }

    public BuffData GetBuffData()
    {
        return _buffData;
    }

    public AttackData GetAttackData()
    {
        return _attackData;
    }

    public bool IsRanged()
    {
        return ranged;
    }

    public object SaveState()
    {
        return new SaveData()
        {
            health = this._currHealth,
            maxHealth = this.maxHealth,
         };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        _currHealth = saveData.health;
        maxHealth = saveData.maxHealth;
        _healthBar.SetHealth(_currHealth);
    }

    [Serializable]
    private struct SaveData
    {
        public int health;
        public int maxHealth;
    }
}