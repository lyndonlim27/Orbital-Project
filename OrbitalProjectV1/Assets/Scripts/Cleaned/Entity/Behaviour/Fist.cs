using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    public override void TurnWeapon(Vector2 movement)
    {
        
    }

    public override void Shoot(EntityBehaviour entity, Vector2 point2Target)
    {
        //EnemyBehaviour enemyBehaviour = (EnemyBehaviour)entity;
        //if (enemyBehaviour != null)
        //{
        //    Destroy(entity.gameObject, 0.5f);
        //}


        //Destroy(entity.gameObject, 0.5f);

        entity.Defeated();
        
    }

}
