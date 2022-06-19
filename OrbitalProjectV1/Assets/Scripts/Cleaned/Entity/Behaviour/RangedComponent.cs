using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedComponent : AttackComponent
{
    public RangedBehaviour rangedBehaviour;
    public List<RangedData> rangeds;
    private LineController lineController;

    protected override void Awake()
    {
        base.Awake();
        lineController = GetComponentInChildren<LineController>();
    }

    public void LaserAttack()
    {

        if (detectionScript.playerDetected)
        {

            if (target.IsDead() || !abletoAttack)
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
    public void SpellAttackSingle()
    {
        List<RangedData> spells = rangeds.FindAll(ranged => !ranged._type.Equals(EntityData.TYPE.PROJECTILE));
        int random = Random.Range(0, spells.Count);
        Debug.Log(spells[0]);
        RangedData selectedAttack = spells[random];
        Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
        Vector2 pos = (Vector2)this.transform.position + dir * 1.5f;
        Quaternion quaternion = selectedAttack._type == RangedData.TYPE.CAST_SELF ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        SettingUpProjectile(selectedAttack, quaternion, pos);

    }

    public void ShootSingle()
    {
        List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
        Debug.Log("THis is bullets: " + bullets);
        Debug.Log(bullets[0]);
        int random = Random.Range(0, bullets.Count);
        RangedData selectedAttack = bullets[random];
        Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
        Vector2 pos = (Vector2)transform.position + dir * 1.5f;
        Quaternion quaternion = selectedAttack._type == RangedData.TYPE.CAST_SELF ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        transform.rotation = quaternion;
        SettingUpProjectile(selectedAttack, quaternion, pos);
    }

    public void ShootSingleSphere()
    {
        List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
        Debug.Log("THis is bullets: " + bullets);
        Debug.Log(bullets[0]);
        int random = Random.Range(0, bullets.Count);
        RangedData selectedAttack = bullets[random];
        StartCoroutine(RadialAttack());
        
    }
        

    public void SummonProjectiles(RangedData selectedAttack)
    {

        int rand = Random.Range(9, 12);
        for (int i = 0; i < rand; i ++)
        {
            Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
            Quaternion quaternion = selectedAttack._type == RangedData.TYPE.CAST_SELF ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.rotation = quaternion;
            Vector2 pos = (Vector2)transform.position + dir * 1.5f;
            SettingUpProjectile(selectedAttack, quaternion, pos);
        }
        parent.resetCooldown();

    }

    public IEnumerator RadialAttack()
    {
        float angle = 0f;
        List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type == EntityData.TYPE.PROJECTILE);
        int random = Random.Range(0, bullets.Count);
        RangedData selectedAttack = bullets[random];
        while(angle < 360f)
        {
            Vector2 pos = (Vector2)transform.position;
            Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
            SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angle), pos);
            angle += 10f;
            yield return new WaitForSeconds(0.2f);
        }
        parent.resetCooldown();

    }

    private void SettingUpProjectile(RangedData rangedData, Quaternion _quaternion, Vector2 pos)
    {
        Debug.Log(rangedData._type);
        RangedBehaviour projectile = poolManager.GetProjectile(rangedData, target.gameObject, this.gameObject);
        projectile.gameObject.transform.position = pos;
        projectile.gameObject.transform.rotation = _quaternion;
        projectile._firer = parent != null ? parent.gameObject : null;
        projectile.gameObject.SetActive(true);
        //projectile.EnableAnimation();

    }
}