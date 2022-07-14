using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DetectionScript),typeof(Animator))]


/// <summary>
/// This is a general class for trap behaviours.
/// It handles the different behaviours for all traps.
/// </summary>
public class TrapBehaviour : ActivatorBehaviour
{
    StateMachine stateMachine;
    RangedComponent ranged;
    public TrapData trapData;
    private BoxCollider2D _boxColl;
    private Player player;
    private Transform _transform;
    private DamageApplier damageApplier;
    [SerializeField] private StateMachine.STATE currstate;

    /** The first instance the gameobject is being activated.
    *  Retrieves all relevant data.
    */
    protected override void Awake()
    {
        base.Awake();
        detectionScript = GetComponentInChildren<DetectionScript>();
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = false;
        animator.runtimeAnimatorController = Resources.Load("Animations/AnimatorControllers/AC_Trap") as RuntimeAnimatorController;
        ranged = GetComponentInChildren<RangedComponent>();
        _boxColl = detectionScript.GetComponent<BoxCollider2D>();
        damageApplier = GetComponentInChildren<DamageApplier>(true);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


    }

    /**
    * For general items which is used in all entities.
    */
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.TRAPINACTIVE, new TrapInActiveState(this, stateMachine));
        stateMachine.AddState(StateMachine.STATE.TRAPACTIVE, new TrapActiveState(this, stateMachine));
        stateMachine.Init(StateMachine.STATE.TRAPINACTIVE, null);

    }


    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    protected override void OnEnable()
    {
        base.OnEnable();
        animator.enabled = false;
        _transform = transform;
        SettingUpCollider();
        SettingUpRotation();
        if (damageApplier != null)
        {
            if (trapData.attackAudios.Count > 0) {
                damageApplier.SettingUpAudio(trapData.attackAudios[0]);
            }
            damageApplier.SettingUpDamage(trapData.damage);
        }
        spriteRenderer.sprite = null;
        animator.enabled = true;
        ranged.rangeds = trapData.rangedDatas;
        ranged.enabled = trapData.ranged;
        
        
    }

    /**
     * Setting Up Rotation of the trap.
     */
    private void SettingUpRotation()
    {
 
        if (trapData.flip)
        {
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }


    }

    /**
     * Setting Up Colliders.
     */
    private void SettingUpCollider()
    {

        if (trapData.ranged)
        {
            Vector2 _size = trapData.sprite.bounds.size;
            _size.x *= 3;
            _boxColl.size = _size;
            _boxColl.offset = new Vector2(-trapData.sprite.bounds.size.x, 0);
        }
        else
        {
            _boxColl.size = trapData.sprite.bounds.size;
            _boxColl.offset = new Vector2(0, 0);
        }

        _boxColl.gameObject.transform.rotation = trapData.quaternion;
    }

    //private void RandomizePlacement()
    //{
    //    if (trapData.horizontal)
    //    {
    //        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 180));
    //    }
    //    List<Vector2> dirs = new List<Vector2> { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };
    //    if (trapData.ranged)
    //    {
    //        _boxColl.size = trapData.sprite.bounds.size;
    //        _boxColl.offset = new Vector2(-trapData.sprite.bounds.size.x, 0);

    //        Vector2 currentface = _transform.localScale;
    //        do
    //        {
    //            Vector2 randdir = dirs[Random.Range(0, dirs.Count - 1)];
    //            currentface.x *= randdir.x;
    //            currentface.y *= randdir.y;
    //            _transform.localScale = currentface;
    //        } while (CheckIfOutsideBounds());

    //    }
    //    else
    //    {
    //        _boxColl.size = trapData.sprite.bounds.size;
    //        _boxColl.offset = new Vector2(0, 0);
    //    }
    //}

    private bool CheckIfOutsideBounds()
    {
        if (currentRoom != null)
        {
            Bounds roombounds = currentRoom.GetRoomAreaBounds();
            return _boxColl.bounds.max.x > roombounds.max.x || _boxColl.bounds.max.x < roombounds.min.x ||
                _boxColl.bounds.max.y > roombounds.max.y || _boxColl.bounds.max.y < roombounds.min.y;
        } else
        {
            return false;
        }
       
    }

    /**
     * Checking for state changes every frame.
     */
    void Update()
    {
        stateMachine.Update();
        currstate = stateMachine.currState;
    }

    /**
     * Activate ranged components of trap.
     */
    public void ActivateRangedComponents()
    {
        if (ranged != null)
        {
            ranged.ShootSingle();
        }
    }

    /**
     * Resetting Animation of trap.
     */
    public void resetAnimation()
    {
        inAnimation = false;
    }

    /**
     * Setting trap stats.
     */
    public override void SetEntityStats(EntityData stats)
    {
        TrapData temp = (TrapData)stats;
        if(stats != null)
        {
            trapData = temp;
        }
    }

    /**
     * Retrieving trap stats.
     */
    public override EntityData GetData()
    {
        return trapData;
    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {

        poolManager.ReleaseObject(this);
        
        
    }
}
