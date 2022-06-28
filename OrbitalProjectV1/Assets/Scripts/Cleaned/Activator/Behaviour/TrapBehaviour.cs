using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DetectionScript),typeof(Animator))]
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
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.TRAPINACTIVE, new TrapInActiveState(this, stateMachine));
        stateMachine.AddState(StateMachine.STATE.TRAPACTIVE, new TrapActiveState(this, stateMachine));
        stateMachine.Init(StateMachine.STATE.TRAPINACTIVE, null);

    }

    private void OnEnable()
    {
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

    private void OnDisable()
    {
        
    }

    private void SettingUpRotation()
    {
        //if (trapData.flip)
        //{
        //    transform.localScale.x *= -1;
        //}
        //transform.rotation = trapData.quaternion;
        //float angle = trapData.quaternion.eulerAngles.z;
        //if (angle > 90f && angle < 270f)
        //{
        //    Debug.Log("???");
        //    Vector2 scale = transform.localScale;
        //    scale.y *= -1;
        //    transform.localScale = scale;
        //}
        if (trapData.flip)
        {
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }


    }

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

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        currstate = stateMachine.currState;
    }

    public void ActivateRangedComponents()
    {
        if (ranged != null)
        {
            ranged.ShootSingle();
        }
    }

    public void resetAnimation()
    {
        inAnimation = false;
    }

    public override void SetEntityStats(EntityData stats)
    {
        TrapData temp = (TrapData)stats;
        if(stats != null)
        {
            trapData = temp;
        }
    }

    public override void Defeated()
    {

        poolManager.ReleaseObject(this);
        
        
    }

    public override EntityData GetData()
    {
        return trapData;
    }
}
