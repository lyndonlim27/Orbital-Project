using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : EntityBehaviour, IDataPersistence, Freezable
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
    private Animator buffanimator;
    private RuntimeAnimatorController healinganimator;
    private Vector2 savePoint;

    private bool _invulnerable;
    private int playerprogress;

    /*
     * Player skills/data
     */
    private BuffBehaviour _buffBehaviour;
    private DebuffBehaviour _debuffBehaviour;
    private AttackSkillBehaviour _attackBehaviour;
    private Shop _shop;

    [Header("Player Data")]
    [SerializeField] private int maxMana;
    [SerializeField] private int maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool ranged;
    [SerializeField] private BuffData _buffData;
    [SerializeField] private DebuffData _debuffData;
    [SerializeField] private AttackData _attackData;
    


    [Header("AudioClips")]
    Dictionary<string,AudioClip> audioClips;



    [Header("Player UI")]
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ManaBar _manaBar;
    public bool InCombat { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
        InCombat = false;
        healinganimator = Resources.Load("Animations/AnimatorControllers/HealBuffVFX") as RuntimeAnimatorController;
    }

    public bool IsDead()
    {
        return this._currHealth <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //currGold = 100;
        currMana = maxMana;
        //_currHealth = maxHealth;
        _currHealth = 100;
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
        _debuffBehaviour = FindObjectOfType<DebuffBehaviour>(true);
        _attackBehaviour = FindObjectOfType<AttackSkillBehaviour>(true);
        _shop = FindObjectOfType<Shop>(true);
        _invulnerable = false;
        isDead = false;
        inAnimation = false;
        audioClips = new Dictionary<string, AudioClip>();
        audioClips["HPMP"] = Resources.Load("Sounds/UI/HPMP") as AudioClip;
        audioClips["Gold"] = Resources.Load("Sounds/UI/Gold") as AudioClip;
        buffanimator = transform.Find("BuffAnimator").gameObject.GetComponent<Animator>();
        savePoint = transform.position;

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
                InCombat = true;
            }
            else
            {
                InCombat = false;
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
    public override void TakeDamage(int damageTaken)
    {
        if (!_invulnerable)
        {
            CameraShake.instance.SetUpShake(4f, .1f);
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
        
        if (_currHealth + health > maxHealth)
        {
            _currHealth = maxHealth;
            
        }
        else
        {
            _currHealth = Math.Min(_currHealth + health, maxHealth);
        }
        _healthBar.SetHealth(_currHealth);
    }

    public void AddGold(int gold)
    {
        currGold += gold;
        _goldCounter.GoldUpdate();
    }

    public void PlayRegen()
    {
        audioSource.Stop();
        audioSource.clip = audioClips["HPMP"];
        audioSource.Play();

    }

    public void PlayRegenAnim()
    {
        buffanimator.runtimeAnimatorController = healinganimator;
        buffanimator.SetTrigger("Activate");

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
    public void Shoot(GameObject target)
    {
        if (_buffBehaviour.inStealth)
        {
            StartCoroutine(_buffBehaviour.Unstealth());
        }
        _timeDelay = 0.5f;
        //Debug.Log(enemy);
        Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _currWeapon.Shoot(target, point2Target);
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
    }


    public void FreezeRb()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnFreezeRb()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
    }

    //Fades sprite
    public override IEnumerator FadeOut()
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

    public void SetDebuffData(DebuffData debuffData)
    {
        this._debuffData = debuffData;
    }

    public void SetBuffData(BuffData buffData)
    {
        this._buffData = buffData;
    }

    public void SetAttackData(AttackData attackData)
    {
        this._attackData = attackData;
    }

    public void SetSavePoint(Vector2 _savepoint)
    {
        this.savePoint = _savepoint;
    }

    public bool IsRanged()
    {
        return ranged;
    }

    public void LoadData(GameData data)
    {
        
        this._currHealth = data.currHealth;
        this.maxHealth = data.maxHealth;
        this.maxMana = data.maxMana;
        this.currMana = data.currMana;
        this.currGold = data.currGold;
        this.transform.position = data.currPos;
        _goldCounter.GoldUpdate();
        if(data.debuffDataName != "")
        {
            _debuffBehaviour.ChangeSkill(data.debuffDataName);
        }

        if (data.attackDataName != "")
        {
            _attackBehaviour.ChangeSkill(data.attackDataName);
        }

        if (data.buffDataName != "")
        {
            _buffBehaviour.ChangeSkill(data.buffDataName);
        }
        _weaponManager.Swap(data.currWeapon);
        _healthBar.SetHealth(_currHealth);
        _manaBar.SetMana(currMana);
        _moveSpeed = data.moveSpeed;
        ranged = data.ranged;
    }

    public void SaveData(ref GameData data)
    {
        data.currHealth = this._currHealth;
        data.maxHealth = this.maxHealth;
        data.maxMana = this.maxMana;
        data.currMana = this.currMana;
        data.currGold = this.currGold;
        data.currWeapon = _weaponManager.ActiveWeapon().name;
        data.currPos = this.savePoint;
        data.debuffDataName = _debuffData != null ? _debuffData.skillName : null;
        data.buffDataName = _buffData != null ? _buffData.skillName : null;
        data.attackDataName = _attackData != null ? _attackData.skillName : null;
        data.moveSpeed = this._moveSpeed;
        data.currScene = SceneManager.GetActiveScene().name;
        data.ranged = this.ranged;
    }

    public void Freeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.speed = 0;
    }

    public void UnFreeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator.speed = 1;
    }
}