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
    public string WeaponName;
  
    

    [Header("Weapon properties")]
   // [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Bullet _bulletPrefab;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

    }

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When weaopn collides with player, pick up weapon
        Player player = collision.GetComponent<Player>();
        if(player)
        {
        player.PickupItem(this);
        }
    }
    


    public void TurnWeapon(Vector2 movement)
    {
        //Turns the weapon to the same direction as the player
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

}
