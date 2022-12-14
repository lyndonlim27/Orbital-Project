using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EntityDataMgt;
using GameManagement;

/**
 * Weapon Behaviour.
 */
namespace EntityCores
{
    public class Weapon : EntityBehaviour
    {
        public Animator _animator { get; private set; }
        public string WeaponName;
        public float range = 200f;

        [Header("Weapon properties")]
        [SerializeField] private RangedBehaviour _bulletPrefab;
        [SerializeField] private RangedData rangedData;

        // Start is called before the first frame update
        protected override void Start()
        {
            _animator = GetComponent<Animator>();
            if (_animator != null)
            {
                _animator.keepAnimatorControllerStateOnDisable = false;
            }

            poolManager = FindObjectOfType<PoolManager>();
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
        public virtual void Shoot(GameObject target, Vector2 point2Target)
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
            InstantiateProjectile(target, point2Target, angle);


        }

        private void InstantiateProjectile(GameObject target, Vector2 point2Target, Quaternion angle)
        {
            RangedBehaviour bullet = poolManager.GetProjectile(rangedData, target, this.gameObject) as RangedBehaviour;
            bullet.gameObject.transform.position = (Vector2)this.transform.position + point2Target * 1.5f;
            bullet.gameObject.transform.rotation = angle;
            bullet.SetEntityStats(rangedData);
            bullet.GetComponent<SpriteRenderer>().sprite = rangedData.sprite;
            bullet.TargetEntity(target.transform.parent.gameObject);
            bullet.gameObject.SetActive(true);
        }

        public virtual void MeleeAttack()
        {

        }

        public override void SetEntityStats(EntityData stats)
        {
            rangedData = (RangedData)stats;
        }

        public override void Defeated()
        {

        }

        private void Update()
        {
            FlipFace();

        }

        private void FlipFace()
        {
            if (_animator != null)
            {
                spriteRenderer.flipX = _animator.GetFloat("Horizontal") < 0;
                spriteRenderer.sortingOrder = _animator.GetFloat("Vertical") >= 1f ? 1 : 3;
            }


        }

        public override EntityData GetData()
        {
            return rangedData;
        }
    }
}
