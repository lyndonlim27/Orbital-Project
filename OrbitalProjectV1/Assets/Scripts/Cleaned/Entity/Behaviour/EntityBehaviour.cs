using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; protected set; }

    protected RoomManager currentRoom;

    protected PoolManager poolManager;

    protected int health;

    public bool isDead;

    public bool inAnimation;

    protected virtual void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract void SetEntityStats(EntityData stats);

    public abstract void Defeated();

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Defeated();
        }
    }

    public abstract EntityData GetData();


    protected virtual IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        //this.gameObject.SetActive(false);
        poolManager.ReleaseObject(this);
    }

    public void SetCurrentRoom(RoomManager roomManager)
    {
        currentRoom = roomManager;
    }
}
