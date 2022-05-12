using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon8Direction : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator _animator;
    private Transform _target;
    int count = 0;
  
    

    [Header("Weapon properties")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Bullet _bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Shoot") || Input.GetMouseButtonDown(0))
        {

            Shoot();

        }
        if (count < 1)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            _animator.SetFloat("Horizontal", movement.x);
            _animator.SetFloat("Vertical", movement.y);
            _animator.SetFloat("Speed", movement.magnitude);
        }
        count--;


    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * _moveSpeed * Time.fixedDeltaTime);


    }

    private void Shoot()
    {
        count = 50;
        Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
        point2Target.Normalize();
        point2Target = -point2Target;
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
        /*
        if (movement.y == 0 && movement.x == 0)
        {
            movement.y = -1;
        }*/

        if (point2Target.y == 0 && point2Target.x == 0)
        {
            point2Target.y = -1;
        }




        Quaternion angle = Quaternion.Euler(0, 0, Mathf.Atan2(point2Target.y, point2Target.x)
                * Mathf.Rad2Deg);
        Bullet bullet = Instantiate(_bulletPrefab, this.transform.position, angle);
        Debug.Log(point2Target);

    }

}
