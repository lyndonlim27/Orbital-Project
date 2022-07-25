using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class MainCamera : MonoBehaviour
{
    Tilemap waterTile;
    Transform tr;
    CinemachineVirtualCamera cam;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        waterTile = GridsHolder.instance.GetTilemap("Water");
        tr = transform;
        cam = GetComponent<CinemachineVirtualCamera>();
        x = cam.m_Lens.OrthographicSize * cam.m_Lens.Aspect;
        y = cam.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        waterTile.transform.position = new Vector2(tr.position.x - x, tr.position.y - y);
    }
}
