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

    protected override void Start()
    {
        spriteRenderer.sprite = data.sprite;
        _animator.runtimeAnimatorController =
            Resources.Load(string.Format("Animations/AnimatorControllers/{0}", data.ac_name)) as RuntimeAnimatorController;

    }

    public override void Defeated()
    {
        _animator.SetTrigger(data._trigger);
        foreach(EntityData entityData in data.entityDatas)
        {
            Debug.Log(entityData.sprite);
            
            entityData.pos = (Vector2) this.transform.position
                + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            entityData.random = false;
            currentRoom.InstantiateEntity(entityData);
           // entity.SetEntityStats(entityData);
           // entity.GetComponent<SpriteRenderer>().sprite = entityData.sprite;
           //Debug.Log(entity.GetData());
            
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

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}



