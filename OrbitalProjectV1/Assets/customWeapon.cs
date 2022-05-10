using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customWeapon : MonoBehaviour
{
    Vector2 rightAttackOffset;
    public CapsuleCollider2D swordCollider;
    public float damage = 3;

    private void Start()
    {
        rightAttackOffset = transform.position;
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;

    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //deal damage to enemy;
            //i need player script.
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.health -= damage;
            }
        }
    }
}
