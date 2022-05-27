using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected RoomManager currentRoom;

    public abstract void SetEntityStats(EntityData stats);

    public abstract void Defeated();

    public abstract EntityData GetData();

    protected virtual void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {

    }

    public void SetCurrentRoom(RoomManager roomManager)
    {
        currentRoom = roomManager;
    }
}
