using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemStats itemStats;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        this.spriteRenderer.sprite = itemStats.sprite;
    }

    public void setItemStats(ItemStats stats)
    {
        this.itemStats = stats;
    }

}
