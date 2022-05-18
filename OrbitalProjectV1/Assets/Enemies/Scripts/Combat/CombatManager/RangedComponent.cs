using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackComponent
{
    public List<Spell> spells;

    public override void Start()
    {
        base.Start();
        spells = enemyStats.spells;
    }

    public override void Attack()
    {

        if (detectionScript.playerDetected)
        {

            if (target.isDead())
            {
                return;
            }
            if (spells.Count != 0) // if there is no spell, nothing to cast here;
            {
                int random = Random.Range(0, spells.Count);

                Spell spell = spells[random];

                //shoot or cast;
                GetComponentInParent<Animator>().SetTrigger(spell.SpellStats.trigger);

                if (spell.SpellStats.type == SpellStats.Type.CAST)
                {
                    // if its a casting spell, no need to aim, just instantiate on top of enemy.
                    GameObject.Instantiate(spell, target.transform.position, Quaternion.identity);
                }
                else
                {
                    // if its a projectile, need to aim.
                    Projectile proj = (Projectile)GameObject.Instantiate(spell, transform.position, Quaternion.identity);
                    proj.SendMessage("SetTarget", target.transform.position);
                }
            }

        }
    }
}

    //private IEnumerator KnockBackCo(Rigidbody2D rb)
    //{
    //    if (rb != null)
    //    {
    //        yield return new WaitForSeconds(knockbackTime);
    //        rb.velocity = Vector2.zero;
    //    }
    //}


