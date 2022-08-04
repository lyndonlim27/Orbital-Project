using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class RotateAroundPoint : MonoBehaviour
//{
//    private Player _player;
//    private Rigidbody2D rb;
//    Vector2 _offsetDirection;
//    float _distance;
//    // Start is called before the first frame update
//    void Start()
//    {
//        _player = FindObjectOfType<Player>();
//        _offsetDirection = transform.position - _player.transform.position;
//        _distance = _offsetDirection.magnitude;
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        transform.Rotate(new Vector3(0, 0, 720) * Time.deltaTime);
//        Quaternion rotate = Quaternion.Euler(0, 0, 200f * Time.deltaTime);
//        _offsetDirection = (rotate * _offsetDirection).normalized;
//        transform.position = (Vector2) _player.transform.position + _offsetDirection * _distance;
//        //transform.RotateAround(_player.transform.position, new Vector3(0, 0, 1), 200 * Time.deltaTime);
//    }
//}



