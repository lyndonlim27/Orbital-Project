using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 500.0f;
    [SerializeField]
    private float lifeTime = 10.0f;
    private Rigidbody2D _rb;
    private Transform _target;
    [SerializeField]
    private float rotateSpeed = 50.0f;
    private Vector2 _movement;

    // Start is called before the first frame update
    void Start()
    {
        _rb.velocity = new Vector2(1, 0);
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        _rb.angularVelocity = rotateSpeed * value;
        _rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
