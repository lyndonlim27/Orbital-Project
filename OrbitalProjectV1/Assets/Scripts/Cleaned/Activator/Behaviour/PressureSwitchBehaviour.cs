using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchBehaviour : ActivatorBehaviour
{ 
    //private bool _status;
    private List<Collider2D> colliders;
    private Coroutine _coroutine = null;
    private float activatedTime;

    void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, 1); }

    protected override void Awake()
    {
        base.Awake();
        
        colliders = new List<Collider2D>();

    }

    private void OnEnable()
    {
        ResettingColor();
        SettingUpColliders();
    }

    private void SettingUpColliders()
    {
        CircleCollider2D body = GetComponent<CircleCollider2D>();
        if (data != null)
        {
            body.radius = data.sprite.bounds.max.x - data.sprite.bounds.center.x;
            body.offset = new Vector2(0f, 0f);

        }
        
    }

    private void Start()
    {
        
    }
    public override EntityData GetData()
    {
        return data;
    }

    public void Update()
    {
        if (!IsOn())
        {
            currentRoom.UnfulfillCondition(data._name);
            spriteRenderer.color = data.defaultcolor;

        } else
        {
            
            currentRoom.FulfillCondition(data._name);
            spriteRenderer.color = data.activatedcolor;
        }

        //Debug.Log("Activatedtime =" + activatedTime);
        //Debug.Log("Currenttime =" + Time.time);
    }

    
    public bool IsOn()
    {
        
        return Time.time <= activatedTime + data.duration;
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {

        
        activatedTime = Time.time;
        //colliders.Add(collider);
        //if (colliders.Count == 1)
        //{
        //    OnSwitch();
        //    if (_coroutine != null)
        //    {
        //        StopCoroutine(_coroutine);
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        activatedTime = Time.time;
    }

    //public void OnTriggerExit2D(Collider2D collider)
    //{

    //    colliders.Remove(collider);
    //    if (colliders.Count == 0)
    //    {
    //        _coroutine = StartCoroutine(offDelay());
    //    }
    //}

    public override void SetEntityStats(EntityData stats)
    {
        this.data = (SwitchData) stats;
    }

    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }



    //private IEnumerator offDelay()
    //{
    //    yield return new WaitForSeconds(data.duration);
    //    OffSwitch();

    //}

    //private void OnSwitch()
    //{
    //    _animator.SetBool("Collision", true);
    //    currentRoom.FulfillCondition(data._name);
    //}

    //private void OffSwitch()
    //{
    //    _animator.SetBool("Collision", false);
    //    currentRoom.UnfulfillCondition(data._name);
    //}

    //public override void Defeated()
    //{
    //}

}
