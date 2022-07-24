using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

/// <summary>
/// This is a general class for pressure switches.
/// It handles the different behaviours for all pressure switches.
/// </summary>

public class PressureSwitchBehaviour : ActivatorBehaviour
{ 
    private CircleCollider2D body;
    private bool entered;
    private Light2D light2D;
    [SerializeField] protected AudioClip audioClip;

    /**
     * Debugging.
     */
    void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, 1); }

    /** The first instance the gameobject is being activated.
     *  Retrieves all relevant data.
     */
    protected override void Awake()
    {
        base.Awake();
        light2D = GetComponent<Light2D>();

    }

    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    protected override void OnEnable()
    {
        base.OnEnable();
        ResettingColor();
        SettingUpColliders();
        CheckForURP();
        spriteRenderer.sortingOrder = 1;
        DestroyAnyObstacles();
    }

    private void DestroyAnyObstacles()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2, LayerMask.GetMask("Obstacles","PassableDeco"));
        foreach(Collider2D col in colliders)
        {
            if (col.CompareTag("Tiles"))
            {
                if (GridsHolder.instance != null)
                {
                    //col.GetComponent<Tilemap>().SetTile(Vector3Int.RoundToInt(col.transform.position), null);
                    GridsHolder.instance.GetTilemap("GroundDecoTilemap").SetTile(Vector3Int.RoundToInt(col.transform.position), null);
                    GridsHolder.instance.GetTilemap("ExteriorTilemap").SetTile(Vector3Int.RoundToInt(col.transform.position), null);
                    GridsHolder.instance.GetTilemap("InteriorTilemap").SetTile(Vector3Int.RoundToInt(col.transform.position), null);
                    GridsHolder.instance.GetTilemap("NearWallTilemap").SetTile(Vector3Int.RoundToInt(col.transform.position), null);
                    DestroyTree(col);
                }
                
            }
            else
            { 
                Destroy(col.gameObject);
            }
        }
    }

    private static void DestroyTree(Collider2D col)
    {
        GridsHolder.instance.GetTilemap("TreesBottomLayer").SetTile(Vector3Int.RoundToInt(col.transform.position), null);
        GridsHolder.instance.GetTilemap("TreesTopLayer1").SetTile(Vector3Int.RoundToInt(col.transform.position) + Vector3Int.up, null);
        GridsHolder.instance.GetTilemap("TreesTopLayer2").SetTile(Vector3Int.RoundToInt(col.transform.position) + Vector3Int.up, null);
        GridsHolder.instance.GetTilemap("TreesTopLayer3").SetTile(Vector3Int.RoundToInt(col.transform.position) + Vector3Int.up, null);
    }

    /**
     * Checking of URP objects.
     */
    private void CheckForURP()
    {
        if (data.NotURP)
        {
            light2D.enabled = false;
        }
    }

    /**
     * Setting Up Colliders.
     */
    private void SettingUpColliders()
    {
        body = GetComponent<CircleCollider2D>();
        if (data != null)
        {
            body.radius = data.sprite.bounds.max.x - data.sprite.bounds.center.x;
            body.offset = new Vector2(0f, 0f);

        }
        
    }

    /**
     * Check to see if pressure switch is activated to change color every frame.
     */
    public void Update()
    {
        if (!IsOn()) 
        {
            entered = false;
            spriteRenderer.color = data.defaultcolor;

        }
        else
        {
            
            if (!entered)
            {
                StartCoroutine(LoadSingleAudio(audioClip));
                entered = true;
            }
            spriteRenderer.color = data.activatedcolor;
        }
    }

    /**
     * Check if pressure switch is turned on.
     */
    public bool IsOn()
    {
        
        return body.IsTouchingLayers(layerMask);
    }

    /**
     * Setting Pressure Switch Data.
     */
    public override void SetEntityStats(EntityData stats)
    {
        data = (SwitchData)stats;
    }

    /**
     * Getting Pressure Switch Data.
     */
    public override EntityData GetData()
    {
        return data;
    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }
}
