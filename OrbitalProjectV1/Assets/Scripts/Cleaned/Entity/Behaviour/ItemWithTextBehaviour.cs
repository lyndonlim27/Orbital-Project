using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWithTextBehaviour : EntityBehaviour
{
   [SerializeField] private ItemWithTextData data;
    private Animator _animator;
    

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isDead = false;

    }

    protected void OnDisable()
    {
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;

    }

    void Start()
    {
        

    }

    public override void Defeated()
    {
        isDead = true;
        foreach (EntityData entityData in data.entityDatas)
        {
            Debug.Log(entityData.sprite);

            entityData.pos = (Vector2)this.transform.position
                + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            entityData.random = false;
            currentRoom.InstantiateEntity(entityData);
            if (entityData.condition == 1)
            {
                currentRoom.UnfulfillCondition(entityData._name);
            }

        }

        if (data.isAWeapon)
        {
            FindObjectOfType<WeaponPickup>().Swap(data._name);
        }

        if (data.condition == 1)
        {
            currentRoom.FulfillCondition(data._name);
        }


        if (_animator.runtimeAnimatorController == null)
        {
            StartCoroutine(FadeOut());
        } else
        {
            _animator.SetTrigger(data._trigger);
        }
        
    }

    public override EntityData GetData()
    {
        return data;
    }

    public override void SetEntityStats(EntityData stats)
    {

        ItemWithTextData temp = (ItemWithTextData)stats;
        Debug.Log("TEMP" + temp);
        if (temp != null)
        {
            data = temp;
        }
    }

    //IEnumerator FadeOut()
    //{
    //    for (float f = 1f; f >= -0.05f; f -= 0.05f)
    //    {
    //        Color c = spriteRenderer.material.color;
    //        c.a = f;
    //        spriteRenderer.material.color = c;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    Destroy(this.gameObject);
    //}
}



