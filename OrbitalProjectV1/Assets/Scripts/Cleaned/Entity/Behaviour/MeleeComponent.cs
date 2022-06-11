using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackComponent
{

    private void OnEnable()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 1);
        GetComponent<BoxCollider2D>().offset = new Vector2(0.5f, 0);
        //Sprite _sprite = parent.spriteRenderer.sprite;
        //Debug.Log(_sprite);
        //Debug.Log(_sprite.bounds.size);
        //var _Collider = GetComponent<BoxCollider2D>();
        //Debug.Log(_Collider);
        //_Collider.size = _sprite.bounds.size;
        //_Collider.offset = new Vector2(0.5f, 0);
        //Debug.Log(_Collider.size);

    }

    public void Attack()

    {
        if (detectionScript.playerDetected)
        {
            if (target.IsDead())
            {
                return;
            }

            Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            target.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);
            target.TakeDamage(enemyData.damageValue);
        }
    }


}
