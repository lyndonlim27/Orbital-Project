using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private Collider2D _collider;
    private AttackData attackData;
    private AttackSkillBehaviour _attackSkill;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _attackSkill = FindObjectOfType<AttackSkillBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.GetComponent<EnemyBehaviour>() != null)
        {
            for (int i = 0; i < _attackSkill._attackData.damage; i++)
            {
                collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(attackData.damage);
            }
        }
        GetComponent<Animator>().SetBool("Explode", true);
    }
}
