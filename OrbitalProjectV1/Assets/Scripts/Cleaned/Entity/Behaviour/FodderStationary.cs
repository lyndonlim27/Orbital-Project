using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderStationary : EnemyBehaviour
{
    public void Start()
    {
        
        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.IDLE, new C_IdleState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK1, new C_MeleeState(this, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.TELEPORT, new C_TeleportState(this, this.stateMachine));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Vector2 direction = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);
            player.TakeDamage(enemyData.damageValue);
        }
    }
}



