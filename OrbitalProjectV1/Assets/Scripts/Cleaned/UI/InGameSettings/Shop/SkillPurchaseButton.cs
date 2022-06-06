using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPurchaseButton : MonoBehaviour
{
    protected Shop shop;
    protected Player player;
    protected SpriteRenderer spriteRenderer;

        

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        shop = FindObjectOfType<Shop>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
