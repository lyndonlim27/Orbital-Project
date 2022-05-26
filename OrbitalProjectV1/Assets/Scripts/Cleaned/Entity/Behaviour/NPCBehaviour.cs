using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;


    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        this.spriteRenderer.sprite = data.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal virtual void Fulfill()
    {
       
    }

    public override void Defeated()
    {
        return;
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.data = (NPCData) stats;
    }

    public override EntityData GetData()
    {
        return data;
    }
}
