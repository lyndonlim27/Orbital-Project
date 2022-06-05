using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackComponent
{
    //public List<Spell> spells; i want to use an interface to generalize all projectiles.. maybe Projectile interface;
    public Spell spell;
    public Bullet bullet;
    public List<RangedData> rangeds;
    //private int cooldown;
    public bool abletoAttack { get; private set; }

    public override void Start()
    {
        base.Start();
        abletoAttack = true;
        //spells = enemyStats.spells;

    }

    public override void Attack()
    {

        if (detectionScript.playerDetected)
        {

            if (target.isDead() || !abletoAttack)
            {
                parent.stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
            }

            abletoAttack = false;
            int random = Random.Range(0, rangeds.Count);
            RangedData rangeddata = rangeds[random];
            if (rangeddata.type == RangedData.Type.CAST_ONTARGET ||
                rangeddata.type == RangedData.Type.CAST_SELF)
            {
                StartCoroutine(Cast(rangeddata));
                return;
            }
            else if (rangeddata.type == RangedData.Type.PROJECTILE)
            {
                StartCoroutine(Shoot(rangeddata));
                return;
            }
        }
        
    }


        public IEnumerator Shoot(RangedData data)
    {
        GetComponentInParent<Animator>().SetTrigger(data.trigger);
        yield return new WaitForSeconds(2f);
        Vector2 dir = (Vector2) (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.SetEntityStats(data);
        bullet.GetComponent<SpriteRenderer>().sprite = data.sprite;
        Bullet bulletinst;
 
        Quaternion direction = Quaternion.Euler(0, 0, angle);
        
        if (parent.stage2)
        {

            bulletinst = GameObject.Instantiate(bullet, (Vector2)transform.position + dir * 2f, Quaternion.Euler(0, 0, angle - 45f));
            bulletinst.TargetEntity(target);
            bulletinst._firer = parent;
            bulletinst = GameObject.Instantiate(bullet, (Vector2)transform.position + dir * 2f, direction);
            bulletinst.TargetEntity(target);
            bulletinst._firer = parent;
            bulletinst = GameObject.Instantiate(bullet, (Vector2)transform.position + dir * 2f, Quaternion.Euler(0, 0, angle + 45f));
            bulletinst.TargetEntity(target);
            bulletinst._firer = parent;
            StartCooldown();
        }
        else
        {
            bulletinst = Instantiate(bullet, (Vector2)transform.position + dir * 2f, direction);
            bulletinst.TargetEntity(target);
            bulletinst._firer = parent;
            StartCooldown();
        }
        
        
     
    }
    // cast on the target.
    public IEnumerator Cast(RangedData data)
    {
        GetComponentInParent<Animator>().SetTrigger(data.trigger);
        Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
        Quaternion angle = data.type == RangedData.Type.CAST_SELF ? Quaternion.Euler(0,0,Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) : Quaternion.identity;
        //EliteMonsterA eliteMonsterA = (EliteMonsterA)parent;
        Spell spellinst;
        if (parent.stage2)
        {
            float ang = 360f;
            while (ang != 0)
            {

                Quaternion direction = Quaternion.Euler(0, 0, ang);
                spellinst = GameObject.Instantiate(spell, this.transform.position, direction);
                spellinst.SetEntityStats(data);
                ang -= 45f;
                yield return new WaitForSeconds(1.5f);

            }
            StartCooldown();
        }
        else
        {
            spellinst = GameObject.Instantiate(spell, this.transform.position, angle);
            spellinst.SetEntityStats(data);
            StartCooldown();
        }
         
    }

    /*
    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        abletoAttack = true;
        parent.resetCooldown();

    }*/

    private void StartCooldown()
    {
        abletoAttack = true;
        parent.resetCooldown();
    }

}


