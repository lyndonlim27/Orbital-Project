using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
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
        //_target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if(this.transform.parent != null)
        {
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    /*void Update()
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


    }*/
    /*
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * _moveSpeed * Time.fixedDeltaTime);


    }*/
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        //this.GetComponent<Collider2D>().enabled = false;
        if(player)
        {
        player.PickupItem(this);
        }
    }
    


    public void TurnWeapon(Vector2 movement)
    {
       // movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");


        _animator.SetFloat("Horizontal", movement.x);
        _animator.SetFloat("Vertical", movement.y);
        _animator.SetFloat("Speed", movement.magnitude);
    }
    
    public void Shoot(Vector2 point2Target)
    {
        
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
        

        if (point2Target.y == 0 && point2Target.x == 0)
        {
            point2Target.y = -1;
        }



        Quaternion angle = Quaternion.Euler(0, 0, Mathf.Atan2(point2Target.y, point2Target.x)
                 * Mathf.Rad2Deg);

        Bullet bullet = Instantiate(_bulletPrefab, this.transform.position, angle);


    }

    public void SetPosition(Transform trans)
    {
        this.transform.localPosition.Set(0,0,0);
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;
        this.transform.localRotation = Quaternion.Euler(Vector2.zero);
        
        Debug.Log(this.transform.parent.name);
    }


}
