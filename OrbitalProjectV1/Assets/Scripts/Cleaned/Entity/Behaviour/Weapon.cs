using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Weapon Behaviour.
 */
public class Weapon : EntityBehaviour
{
    public Animator _animator { get; private set; }
    public string WeaponName;
    public float range = 200f;  

    [Header("Weapon properties")]
    [SerializeField] private Bullet _bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /** 
     * Turns the weapon direction.
     */
    public virtual void TurnWeapon(Vector2 movement)
    {
        //Turns the weapon to the same direction as the player
        _animator.SetFloat("Horizontal", movement.x);
        _animator.SetFloat("Vertical", movement.y);
        _animator.SetFloat("Speed", movement.magnitude);
    }

    /** 
     * Turns the weapon direction towards the enemy and shoot
     */
    public virtual void Shoot(EntityBehaviour entity, Vector2 point2Target)
    {
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
        bullet.TargetEntity(entity);

    }

    public virtual void MeleeAttack()
    {

    }

    public override void SetEntityStats(EntityData stats)
    {
        throw new System.NotImplementedException();
    }

    public override void Defeated()
    {

    }

    public override EntityData GetData()
    {
        throw new System.NotImplementedException();
    }
}
