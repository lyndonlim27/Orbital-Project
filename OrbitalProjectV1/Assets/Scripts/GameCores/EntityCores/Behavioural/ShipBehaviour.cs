using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace EntityCores
{
    public class ShipBehaviour : MonoBehaviour
    {
        Rigidbody2D _rb;
        private float speed = 10f;
        private float startRotate;
        [SerializeField] private bool rotate;
        private float rotatedAngle;
        private float duration = 3f;
        private Vector3 currdir;
        private float enterTime;
        private Vector2 prevDirec;
        private Tilemap waterTilemap;
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            rotate = false;
            currdir = transform.right.normalized;
            waterTilemap = GameObject.Find("Water").GetComponent<Tilemap>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {

            transform.position += speed * -transform.right * Time.deltaTime;

            if (transform.position.x <= waterTilemap.cellBounds.xMin)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180f);
            }
            else if (transform.position.x >= waterTilemap.cellBounds.xMax)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
