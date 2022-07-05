using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBehaviour : MonoBehaviour
{
    private float cooldown = 2f;
    private int healingpertick = 15;
    private int manapertick = 15;
    private bool healing;
    private Collider2D _col;
    private Player player;
    private RoomManager roomManager;

    private void OnEnable()
    {
        foreach (Transform _tr in GetComponentsInChildren<Transform>(true))
        {
            _tr.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            healing = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            healing = false;
        }
    }

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
       player = FindObjectOfType<Player>(true);
    }

    private void Update()
    {
        
        if (healing)
        {
            if (_col.IsTouchingLayers(LayerMask.GetMask("Player")) && cooldown <= 0)
            {
                player.AddHealth(healingpertick);
                player.AddMana(manapertick);
                player.PlayRegen();
                player.PlayRegenAnim();
                resetCooldown();
            }
            else
            {
                tick();
            }
        }    
        
    }

    private void tick()
    {
        cooldown -= Time.deltaTime;
    }

    private void resetCooldown()
    {
        cooldown = 2f;
    }


}
