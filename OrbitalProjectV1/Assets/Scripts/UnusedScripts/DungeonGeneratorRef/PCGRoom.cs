using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//public class PCGRoom : RoomManager
//{
//    Tilemap tilemap;
//    TileBase[] tiles[];

//    private void SetUpRoomSize(Vector2 center, Vector2 size)
//    {
//        roomSize = size;
//        transform.position = center;
//        SetUpPolyColliderSize(center, size);
//    }

//    private void SetUpPolyColliderSize(Vector2 center, Vector2 size)
//    {
//        roomArea = gameObject.AddComponent<PolygonCollider2D>();
//        roomArea.points = new Vector2[]{
//            new Vector2(center.x - size.x / 2, center.y - size.y / 2),
//            new Vector2(center.x + size.x / 2, center.y - size.y / 2),
//            new Vector2(center.x + size.x / 2, center.y + size.y / 2),
//            new Vector2(center.x - size.x / 2, center.y + size.y / 2),
//        };
            
//    }

//    private void BuildRoom()
//    {
        
        
//    }

//    private void BuildWall()
//    {
//        Vector2 bottomleft = roomArea.points[0];
//        Vector2 bottomright = roomArea.points[1];
//        Vector2 topleft = roomArea.points[2];
//        Vector2 topright = roomArea.points[3];

//        /**
//         * Move right
//         */

//        for (int i = (int) bottomleft.x; i <= (int) bottomright.x; i++)
//        {

//        }
        
        

//    }
//}
