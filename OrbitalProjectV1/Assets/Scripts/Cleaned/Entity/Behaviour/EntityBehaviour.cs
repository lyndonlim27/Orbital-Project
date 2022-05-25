using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    public EntityData entityData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        this.spriteRenderer.sprite = entityData.sprite;
    }

    public void SetEntityStats(EntityData stats)
    {
        this.entityData = stats;
    }

    public abstract void Defeated();
}
