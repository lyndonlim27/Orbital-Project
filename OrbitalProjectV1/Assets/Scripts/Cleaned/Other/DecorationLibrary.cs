using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecorationLibrary : MonoBehaviour
{

    public static DecorationLibrary instance { get; private set; }

    [Header("TownDecoratives")]
    [SerializeField]
    private TileBase[] ExtDecorations;
    [SerializeField]
    private TileBase[] InteriorDecorations;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public TileBase[] GetExtDecorations()
    {
        return ExtDecorations;
    }

    public TileBase[] GetInteriorDecorations()
    {
        return InteriorDecorations;
    }

}
