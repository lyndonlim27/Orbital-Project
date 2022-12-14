using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using EntityCores.Behavioural;
using GameManagement;
using EntityDataMgt;
using EntityCores.PlayerCore;
using GameManagement.UIComps;

namespace EntityCores
{

    [RequireComponent(typeof(EntityAudioComp))]
    public class Player : EntityBehaviour, IDataPersistence, Freezable
    {

        public static Player instance { get; private set; }

        public Vector2 _movement;
        public Vector2 lastdirection;
        public Transform _transform;
        public bool insidePuzzle;
        public Rigidbody2D _rb { get; private set; }
        private Collider2D col;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private float _time = 0;
        private float _timeDelay = 0;
        private DialogueManager dialMgr;
        private Weapon originalWeapon;
        private int _currHealth;
        public int currMana { get; private set; }
        private Weapon _currWeapon;
        private WeaponPickup _weaponManager;
        private DamageFlicker _flicker;
        private GoldCounter _goldCounter;
        public int currGold;
        private Animator buffanimator;
        private RuntimeAnimatorController healinganimator;
        private Vector2 savePoint;
        private int _fragments;


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
        private EntityAudioComp audioComp;
        Dictionary<string, AudioClip> audioClips;



        [Header("Player UI")]
        [SerializeField] private GameOver _gameOver;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ManaBar _manaBar;
        [SerializeField] private FragmentUI _fragmentUI;
        public bool InCombat;

        protected override void Awake()
        {
            base.Awake();
            if (instance == null)
            {
                instance = this;
            }
            col = GetComponent<Collider2D>();
            InCombat = false;
            healinganimator = Resources.Load("Animations/AnimatorControllers/HealBuffVFX") as RuntimeAnimatorController;
            _healthBar = FindObjectOfType<HealthBar>(true);
            _manaBar = FindObjectOfType<ManaBar>(true);
            _gameOver = FindObjectOfType<GameOver>(true);
            _fragmentUI = FindObjectOfType<FragmentUI>(true);
            _transform = transform;
        }

        public bool IsDead()
        {
            return this._currHealth <= 0;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
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
            _flicker = DamageFlicker.instance;
            dialMgr = DialogueManager.instance;
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
            insidePuzzle = false;
            audioComp = GetComponent<EntityAudioComp>();
        }

        // Update is called once per frame
        void Update()
        {
            _time += 1f * Time.deltaTime;

            if (_currHealth <= 0)
            {
                Defeated();
            }

            CheckCombat();

            if (_time >= _timeDelay)
            {
                _movement.x = Input.GetAxisRaw("Horizontal");
                _movement.y = Input.GetAxisRaw("Vertical");
                if (_movement != Vector2.zero)
                {
                    lastdirection = _movement;
                }
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
                if (Input.GetKeyDown((KeyCode)i) && !InCombat)
                {
                    StartCoroutine(InitCombat());
                }

            }
        }

        private IEnumerator InitCombat()
        {
            InCombat = true;
            yield return new WaitForSeconds(2f);
            InCombat = false;
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
            audioComp.PlaySingleAudio(audioClips["HPMP"]);
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
            if (currMana + mana > maxMana)
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
            for (float f = 1f; f >= -0.05f; f -= 0.05f)
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
            _gameOver.Setup();
            this.gameObject.SetActive(false);
        }

        public override void SetEntityStats(EntityData stats)
        {
        }

        public override EntityData GetData()
        {
            return null;
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

        public RangedData GetWeaponData()
        {
            return _currWeapon.GetData() as RangedData;
        }

        public void SetWeapon(Weapon weapon)
        {
            _currWeapon = weapon;
        }

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
            this._fragments = data.fragments;
            if (_fragmentUI != null)
            {
                _fragmentUI.SetFragmentUI(data.fragments);
            }
            _goldCounter.GoldUpdate();
            if (data.debuffDataName != "")
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
            _healthBar.SetMaxHealth(maxHealth);
            _manaBar.SetMana(currMana);
            _manaBar.SetMaxMana(maxMana);
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
            data.fragments = this._fragments;
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

        public void SetStats(GameData gameData)
        {
            maxHealth = gameData.maxHealth;
            maxMana = gameData.maxMana;
            _currHealth = maxHealth;
            currMana = maxMana;
            currGold = gameData.currGold;
            _weaponManager.Swap(gameData.currWeapon);
            _moveSpeed = gameData.moveSpeed;
            if (gameData.debuffDataName != "")
            {
                _debuffBehaviour.ChangeSkill(gameData.debuffDataName);
            }

            if (gameData.attackDataName != "")
            {
                _attackBehaviour.ChangeSkill(gameData.attackDataName);
            }

            if (gameData.buffDataName != "")
            {
                _buffBehaviour.ChangeSkill(gameData.buffDataName);
            }
            ranged = gameData.ranged;


        }

        public bool IncreaseMaxHealth()
        {
            if (maxHealth < 150)
            {
                maxHealth += 5;
                _healthBar.SetMaxHealth(maxHealth);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IncreaseMaxMana()
        {
            if (maxMana < 150)
            {
                maxMana += 5;
                _manaBar.SetMaxMana(maxMana);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IncreaseSpeed()
        {
            if (_moveSpeed < 8)
            {
                _moveSpeed += 0.3f;
                _moveSpeed = Mathf.Round(_moveSpeed * 100.0f) * 0.01f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetMaxMana()
        {
            return maxMana;
        }
        public float GetSpeed()
        {
            return _moveSpeed;
        }

        public void SwitchToFist()
        {

            originalWeapon = _currWeapon;


            _weaponManager.Swap("Fist");


        }

        public void SwitchBack()
        {
            _weaponManager.Swap(originalWeapon.WeaponName);
        }

        public void AddFragments()
        {
            _fragments += 1;
            _fragmentUI.SetFragmentUI(_fragments);
        }
    }
}