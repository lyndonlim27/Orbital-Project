using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackComponent
{
    public RangedBehaviour rangedBehaviour;
    public List<RangedData> rangeds;

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

    public void SpiralAttack()
    {
        List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type == EntityData.TYPE.PROJECTILE);
        int random = Random.Range(0, bullets.Count);
        RangedData selectedAttack = bullets[random];

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