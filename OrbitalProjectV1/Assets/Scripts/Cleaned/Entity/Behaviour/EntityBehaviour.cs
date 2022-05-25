using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    public abstract void SetEntityStats(EntityData stats);

    public abstract void Defeated();

    public abstract EntityData GetData();
}
