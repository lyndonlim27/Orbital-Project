using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCores.PlayerCore;
using EntityDataMgt;

namespace EntityCores.Behavioural
{
    public class DestroyOnCollision : MonoBehaviour
    {
        private Collider2D _collider;
        private AttackData attackData;
        private AttackSkillBehaviour _attackSkill;
        private Player player;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<Collider2D>();
            _attackSkill = FindObjectOfType<AttackSkillBehaviour>();
            player = FindObjectOfType<Player>();
            attackData = player.GetAttackData();
            this.gameObject.layer = LayerMask.NameToLayer("PlayerSkill");
            this.gameObject.tag = "PlayerSkill";
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponentInChildren<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackData.damage);
                //for (int i = 0; i < _attackSkill._attackData.damage; i++)
                //{
                //    collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(attackData.damage);
                //}
            }
            GetComponent<Animator>().SetBool("Explode", true);
        }
    }
}
