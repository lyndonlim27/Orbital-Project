using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores.Weapons
{
    public class Fist : Weapon
    {
        public override void TurnWeapon(Vector2 movement)
        {

        }

        public override void Shoot(GameObject target, Vector2 point2Target)
        {
            //EnemyBehaviour enemyBehaviour = (EnemyBehaviour)entity;
            //if (enemyBehaviour != null)
            //{
            //    Destroy(entity.gameObject, 0.5f);
            //}


            //Destroy(entity.gameObject, 0.5f);
            if (target.GetComponent<EntityBehaviour>() != null)
            {
                target.GetComponent<EntityBehaviour>().TakeDamage(0);
            }


        }

    }
}
