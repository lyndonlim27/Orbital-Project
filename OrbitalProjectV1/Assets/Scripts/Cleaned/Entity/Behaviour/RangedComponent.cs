using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackComponent
{
    //public List<Spell> spells; i want to use an interface to generalize all projectiles.. maybe Projectile interface;
    public LightningSpell spell;
    public Bullet bullet;

    public override void Start()
    {
        base.Start();
        //spells = enemyStats.spells;
    }

    public override void Attack()
    {

        if (detectionScript.playerDetected)
        {

            if (target.isDead())
            {
                return;
            }
            //if (spells.Count != 0) // if there is no spell, nothing to cast here;
            if (spell == null && bullet == null)
            {
                return;
            } else if (spell == null)
            {
                Shoot();
                return;
            } else if (bullet == null)
            {
                Cast();
                return;
            }
            else 
            {
                int random = Random.Range(0, 2);
                if (random == 0) {
                    Cast();
                    return;
                }
                else
                {

                    Shoot();
                    return;
                }
            }

        }
    }

    private void Shoot()
    {
        Bullet bulletinst = Instantiate(bullet, this.transform.position, Quaternion.identity);
        bulletinst.TargetEntity(target);
    }

    private void Cast()
    {
        GetComponentInParent<Animator>().SetTrigger(spell.rangedData.ac_name);
        GameObject.Instantiate(spell, target.transform.position, Quaternion.identity);
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


