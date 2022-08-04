using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement.Helpers;
using EntityDataMgt;
using Random = UnityEngine.Random;

namespace EntityCores.Behavioural {
    public class RangedComponent : AttackComponent
    {
        public RangedBehaviour rangedBehaviour;
        public List<RangedData> rangeds;
        private LineController lineController;
        private Patterns patterns;
        private Transform _transform;


        protected override void Awake()
        {
            base.Awake();
            lineController = GetComponentInChildren<LineController>();

        }

        public void LaserAttack()
        {

            lineController.StartCoroutine(lineController.AnimateLine());

        }
        public void SpellAttackSingle()
        {
            List<RangedData> spells = rangeds.FindAll(ranged => !ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, spells.Count);
            RangedData selectedAttack = spells[random];
            Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
            Vector2 pos = selectedAttack._type == RangedData.TYPE.CAST_SELF ? (Vector2)this.transform.position +
                new Vector2(parent.facingRight ? selectedAttack.pos.x * -1f : selectedAttack.pos.x, selectedAttack.pos.y) : (Vector2)this.transform.position + dir;
            Quaternion quaternion = selectedAttack.stationary ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.rotation = quaternion;
            SettingUpProjectile(selectedAttack, quaternion, pos);

        }

        public void SpellAttackSelf()
        {
            List<RangedData> spells = rangeds.FindAll(ranged => !ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, spells.Count);
            RangedData selectedAttack = spells[random];
            SettingUpProjectile(selectedAttack, Quaternion.identity, transform.position);
            parent.ResetDash();
        }

        public void SpellAttackMultiple()
        {
            List<RangedData> spells = rangeds.FindAll(ranged => !ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, spells.Count);
            RangedData selectedAttack = spells[random];
            StartCoroutine(RadialAttack(selectedAttack));
        }

        public void ShootSingle()
        {
            List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, bullets.Count);
            RangedData selectedAttack = bullets[random];
            Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
            Vector2 pos = (Vector2)transform.position + selectedAttack.pos; /*+ dir * 1.5f*/
            Quaternion quaternion = selectedAttack._type == RangedData.TYPE.CAST_SELF ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.rotation = quaternion;

            SettingUpProjectile(selectedAttack, quaternion, pos);
        }

        public void ShootSingleSphere()
        {
            List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, bullets.Count);
            RangedData selectedAttack = bullets[random];
            StartCoroutine(RadialAttack(selectedAttack));

        }

        public void ShootSingleSphereDash1()
        {
            List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, bullets.Count);
            RangedData selectedAttack = bullets[random];
            StartCoroutine(RadialAttackSlow(selectedAttack));

        }

        public void ShootSingleSphereDash2()
        {
            List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type.Equals(EntityData.TYPE.PROJECTILE));
            int random = Random.Range(0, bullets.Count);
            RangedData selectedAttack = bullets[random];
            StartCoroutine(RadialAttackSlowDoubleProj(selectedAttack));

        }




        public void LineProjectile(RangedData selectedAttack)
        {

            int rand = Random.Range(9, 12);
            for (int i = 0; i < rand; i++)
            {
                Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
                Quaternion quaternion = selectedAttack._type == RangedData.TYPE.CAST_SELF ? Quaternion.identity : Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                transform.rotation = quaternion;
                Vector2 pos = (Vector2)transform.position + dir * 1.5f;
                SettingUpProjectile(selectedAttack, quaternion, pos);
            }
            parent.resetCooldown();

        }

        public IEnumerator RadialAttack(RangedData selectedAttack)
        {
            float angle = 0f;

            while (angle < 360f)
            {
                Vector2 pos = (Vector2)transform.position;
                //Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
                //Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg 
                SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, angle), pos);
                angle += 30f;
                yield return null;
                //yield return new WaitForSeconds(0.2f);
            }
            parent.resetCooldown();

        }

        public IEnumerator RadialAttackSlow(RangedData selectedAttack)
        {
            float angle = 0f;

            while (angle < 360f)
            {
                Vector2 pos = (Vector2)transform.position;
                //Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
                //Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg 
                SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, angle), pos);
                angle += 30f;
                //yield return null;
                yield return new WaitForSeconds(0.2f);
            }
            parent.ResetDash();

        }

        public IEnumerator RadialAttackSlowDoubleProj(RangedData selectedAttack)
        {
            float angle = 0f;

            while (angle < 360f)
            {
                Vector2 pos = (Vector2)transform.position;
                //Vector2 dir = (Vector2)(target.transform.position - transform.position).normalized;
                //Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg 
                SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, angle), pos);
                yield return new WaitForSeconds(0.1f);
                SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, angle + 15f), pos);
                angle += 30f;
                //yield return null;
                yield return new WaitForSeconds(0.2f);
            }
            parent.ResetDash();

        }


        public void ShootRandomPattern(List<Vector2> targets)
        {

            List<RangedData> bullets = rangeds.FindAll(ranged => ranged._type == EntityData.TYPE.PROJECTILE);
            int random = Random.Range(0, bullets.Count);
            RangedData selectedAttack = bullets[random];
            foreach (Vector2 target in targets)
            {
                Vector2 dir = (target - (Vector2)transform.position).normalized;
                SettingUpProjectile(selectedAttack, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg), _transform.position);
            }


        }

        private void SettingUpProjectile(RangedData rangedData, Quaternion _quaternion, Vector2 pos)
        {
            RangedBehaviour projectile = poolManager.GetProjectile(rangedData, target.gameObject, this.gameObject);
            projectile.gameObject.transform.position = pos;
            projectile.gameObject.transform.rotation = _quaternion;
            projectile._firer = parent != null ? parent.gameObject : null;
            projectile.gameObject.SetActive(true);
            //projectile.EnableAnimation();

        }
    }
}