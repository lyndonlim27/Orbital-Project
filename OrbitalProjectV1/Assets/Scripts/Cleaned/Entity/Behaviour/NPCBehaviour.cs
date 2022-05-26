using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;
    protected bool fulfilled;

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
        fulfilled = true;
        this.GetComponentInChildren<DialogueDetection>().gameObject.SetActive(false);
        this.GetComponent<Animator>().enabled = false;
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
