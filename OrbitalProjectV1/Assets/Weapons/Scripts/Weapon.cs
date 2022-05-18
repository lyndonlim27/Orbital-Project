using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private Animator _animator;
    public string WeaponName;
    public float range = 200f;

    [Header("Weapon properties")]
    [SerializeField] private Bullet _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }


    //When weaopn collides with player, pick up weapon
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player)
        {
        player.PickupItem(this.gameObject.name);
        Destroy(this.gameObject);
        }
       
    }
    

    //Turns the weapon direction
    public void TurnWeapon(Vector2 movement)
    {
        //Turns the weapon to the same direction as the player
        _animator.SetFloat("Horizontal", movement.x);
        _animator.SetFloat("Vertical", movement.y);
        _animator.SetFloat("Speed", movement.magnitude);
    }

    //Turns the weapon direction towards the enemy and shoot
    //public void Shoot(Entity enemy)
    public void Shoot(Entity enemy, Vector2 point2Target)
    {
        /*
        float _x = enemy.transform.position.x;
        float _y = enemy.transform.position.y;
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(_x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(_y));
        _animator.SetFloat("Speed", enemy.transform.position.magnitude);

        if (_x == 0 && _y == 0)
        {
            _y = -1;
        }

        Quaternion angle = Quaternion.Euler(0, 0, Mathf.Atan2(_y, _x)
                 * Mathf.Rad2Deg);

        Bullet bullet = Instantiate(_bulletPrefab, this.transform.position, angle);
        bullet.SendMessage("TargetEnemy", enemy);
        */
        _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
        _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
        _animator.SetFloat("Speed", point2Target.magnitude);
        if (point2Target.x == 0 && point2Target.y == 0)
        {
            point2Target.y = -1;
        }
        Quaternion angle = Quaternion.Euler(0, 0, Mathf.Atan2(point2Target.y, point2Target.x)
         * Mathf.Rad2Deg);

        Bullet bullet = Instantiate(_bulletPrefab, this.transform.position, angle);
        bullet.TargetEnemy(enemy);

    }

    public void Attack()
    {

    }


}
