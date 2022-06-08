using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected RoomManager currentRoom;

    protected PoolManager poolManager;

    public abstract void SetEntityStats(EntityData stats);

    public abstract void Defeated();

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
        Defeated();
    }

    public void SetCurrentRoom(RoomManager roomManager)
    {
        currentRoom = roomManager;
    }
}
