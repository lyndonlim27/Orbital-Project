using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameManagement.Helpers
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HouseExteriorDesign : HouseDesign
    {
        DecorationLibrary decorationLibrary;
        public BoxCollider2D col;
        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            decorationLibrary = DecorationLibrary.instance;
            SpawnDecorations(decorationLibrary.GetExtDecorations(), false, Vector2Int.RoundToInt(col.bounds.min), Vector2Int.RoundToInt(col.bounds.max), LayerMask.GetMask("HouseInterior", "HouseExterior"));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
    }
}
