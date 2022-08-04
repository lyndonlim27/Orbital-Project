using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerSkill" || collision.gameObject.tag == "Projectile")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                int x = (int)(contact.point.x - 0.01f * contact.normal.x);
                int y = (int)(contact.point.y - 0.01f * contact.normal.y);
                Vector3Int hitpos = new Vector3Int(x, y);
                DestroyTile(hitpos);
            }

        }

    }

    public void DestroyTile(Vector3Int hitpos)
    {
        if (tilemap.name == "TreesBottomLayer")
        {
            DestroyTree(hitpos);
        }
        tilemap.SetTile(tilemap.WorldToCell(hitpos), null);
    }

    protected void DestroyTree(Vector3Int currpost)
    {
        Tilemap tilemap = GridsHolder.instance.GetTilemap("TreesBottomLayer");
        GridsHolder.instance.GetTilemap("TreesBottomLayer").SetTile(tilemap.WorldToCell(currpost), null);
        GridsHolder.instance.GetTilemap("TreesTopLayer1").SetTile(tilemap.WorldToCell(currpost + Vector3Int.up), null);
        GridsHolder.instance.GetTilemap("TreesTopLayer2").SetTile(tilemap.WorldToCell(currpost + Vector3Int.up), null);
        GridsHolder.instance.GetTilemap("TreesTopLayer3").SetTile(tilemap.WorldToCell(currpost + Vector3Int.up), null);
    }

}
