using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom3 : RoomManager
{
    int cooldown;
    //ConsumableItemData consumables;

    public PolygonCollider2D consumableSpawn;
    private Vector2 consumableminArea;
    private Vector2 consumablemaxArea;
    [SerializeField] private ConsumableItemBehaviour consumablePrefab;
    [SerializeField] List<ConsumableItemData> consumables;
    [SerializeField] private HealthBarEnemy bosshp;

    public override void FulfillCondition(string key)
    {
        conditions.Remove(key);
    }

    public override void UnfulfillCondition(string key)
    {
        conditions.Add(key);
    }

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 2000;
        consumableminArea = consumableSpawn.bounds.min;
        consumablemaxArea = consumableSpawn.bounds.max;
    }

    protected override void Update()
    {
        if (activated && conditions.Count != 0)
        {
            bosshp.gameObject.SetActive(true);
            if (cooldown == 0)
            {
                InstantiateConsumables();
                ResetCooldown();
            }
            else
            {
                cooldown--;
            }
        }

        base.Update();
        RoomChecker();
        
    }

    private void InstantiateConsumables()
    {
        foreach (ConsumableItemData data in consumables)
        {
            Vector2 randomPoint = GetRandomPoint(consumableminArea, consumablemaxArea);
            consumablePrefab.SetEntityStats(data);
            consumablePrefab.GetComponent<SpriteRenderer>().sprite = data.sprite;
            Instantiate(consumablePrefab, randomPoint, Quaternion.identity).SetCurrentRoom(this);
        }
        
        
    }

    private void ResetCooldown()
    {
        cooldown = 2000;
    }

}
