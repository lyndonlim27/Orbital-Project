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
    private CircleCollider2D body;
    private bool entered;
    [SerializeField] protected AudioClip audioClip;

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
        spriteRenderer.sortingOrder = 0;
    }
    //specifically for this
    private void SettingUpColliders()
    {
        body = GetComponent<CircleCollider2D>();
        if (data != null)
        {
            body.radius = data.sprite.bounds.max.x - data.sprite.bounds.center.x;
            body.offset = new Vector2(0f, 0f);

        }
        
    }

    public override EntityData GetData()
    {
        return data;
    }

    public void Update()
    {
        if (!IsOn()) 
        {
            entered = false;
            spriteRenderer.color = data.defaultcolor;

        }
        else
        {
            
            if (!entered)
            {
                StartCoroutine(LoadSingleAudio(audioClip));
                entered = true;
            }
            spriteRenderer.color = data.activatedcolor;
        }
    }


    public bool IsOn()
    {
        
        return body.IsTouchingLayers(layerMask);
    }

    public override void SetEntityStats(EntityData stats)
    {
        data = (SwitchData)stats;
    }

    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }
}
