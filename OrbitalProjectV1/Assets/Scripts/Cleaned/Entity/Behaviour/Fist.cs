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
        Destroy(entity.gameObject, 0.5f);
    }

}
