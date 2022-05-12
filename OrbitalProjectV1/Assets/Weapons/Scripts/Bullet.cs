using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform _target;
    private Animator _animator;

    [Header("Bullet properties")]
    [SerializeField] private float speed = 500.0f;
    [SerializeField] private float lifeTime = 10.0f;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 50.0f;

    

    // Start is called before the first frame update
    void Start()
    {
       // _rb.velocity = new Vector2(1, 0);
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        _animator = GetComponent<Animator>();
    }



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        //When the bullet collide with the enemy, stop the movement of the bullet
        if(_animator.GetBool("Collision") == true)
        {
            _rb.angularVelocity = 0;
            _rb.velocity = Vector2.zero;
            return;
        }
        //The bullet will follow the target
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        float value = Vector3.Cross(point2Target, transform.right).z;
        _rb.angularVelocity = rotateSpeed * value;
        _rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Setting collision to true so that it will trigger the next state
        //for bullet and thus play the explosion animation
        _animator.SetBool("Collision", true);
    }

}
