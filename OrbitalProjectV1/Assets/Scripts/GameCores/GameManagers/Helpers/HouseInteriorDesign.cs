using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.Helpers
{
    public class HouseInteriorDesign : HouseDesign
    {

        DecorationLibrary decorationLibrary;
        // Start is called before the first frame update
        void Start()
        {
            decorationLibrary = DecorationLibrary.instance;
            Collider2D col = GetComponent<Collider2D>();
            SpawnDecorations(decorationLibrary.GetInteriorDecorations(), true, Vector2Int.RoundToInt(col.bounds.min), Vector2Int.RoundToInt(col.bounds.max), LayerMask.GetMask("HouseInterior"));
        }

    }
}
