using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedBehaviour : EntityBehaviour
{
    protected Player player;
    public RangedData rangedData;
    protected Animator _animator;


    protected virtual void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void SetEntityStats(EntityData stats)
    {
        RangedData rangedD = Instantiate((RangedData)stats);
        if (rangedD != null)
        {
            this.rangedData = rangedD;
        }
    }

    public override void Defeated()
    {
        Debug.Log(this.gameObject);
        Destroy(this.gameObject);

    }

    public override EntityData GetData()
    {
        return this.rangedData;
    }
}
