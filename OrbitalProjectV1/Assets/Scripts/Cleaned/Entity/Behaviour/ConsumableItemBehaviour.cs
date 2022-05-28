using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItemBehaviour : EntityBehaviour
{
    [SerializeField] private ConsumableItemData _itemData;

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
            Defeated();
        }
    }
}
