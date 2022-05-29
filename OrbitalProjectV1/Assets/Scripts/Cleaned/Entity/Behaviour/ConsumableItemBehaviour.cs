using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItemBehaviour : EntityBehaviour
{
    [SerializeField] private ConsumableItemData _itemData;
    
    [SerializeField] AudioSource soundeffect;


    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.sprite = _itemData.sprite;
    }
    public override void Defeated()
    {

        Destroy(this.gameObject);
    }

    public override EntityData GetData()
    {   
        return _itemData;
    }

    public override void SetEntityStats(EntityData stats)
    {
        this._itemData = (ConsumableItemData) stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            switch (_itemData._consumableType)
            {
                default:
                case ConsumableItemData.CONSUMABLE.HEALTH:
                    player.AddHealth(_itemData._health);
                    break;
                case ConsumableItemData.CONSUMABLE.GOLD:
                    player.AddGold(_itemData._gold);
                    break;
            }
            soundeffect.Play();
            StartCoroutine(FadeOut(soundeffect.clip.length));
            
        }
    }
    //Fades sprite
    IEnumerator FadeOut(float f)
    {
        for (float g = f; g >= -0.05f; g-= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = g;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Defeated();
    }


}
