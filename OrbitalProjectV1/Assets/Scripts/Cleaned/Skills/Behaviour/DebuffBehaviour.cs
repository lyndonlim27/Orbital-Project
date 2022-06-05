using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffBehaviour : SkillBehaviour
{
    private DebuffData _debuffData;
    private ParticleSystem _slowParticle;
    private ParticleSystem _stunParticle;
    public override void ActivateSkill()
    {
        if (_debuffData != null && CanCast())
        {
            _player.UseMana(_debuffData.manaCost);
            switch (_debuffData.debuffType)
            {
                default:
                case DebuffData.DEBUFF_TYPE.STUN:
                    debuffAnimator.runtimeAnimatorController =
                        Resources.Load<AnimatorOverrideController>("Animations/AnimatorControllers/StunDebuffVFX");
                    debuffAnimator.SetTrigger("Activate");
                    StartCoroutine(Stun());
                    break;
                case DebuffData.DEBUFF_TYPE.SLOW:
                    debuffAnimator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/SlowDebuffVFX");
                    debuffAnimator.SetTrigger("Activate");
                    StartCoroutine(Slow());
                    break;
            }
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _debuffData = (DebuffData)_skillData;
        _slowParticle = Resources.Load<ParticleSystem>("ParticlePrefab/SlowDebuff");
        _stunParticle = Resources.Load<ParticleSystem>("ParticlePrefab/StunDebuff");

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    private IEnumerator Stun()
    {
        Debug.Log("STUN");
        ResetCooldown();
        foreach(EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            enemy.GetComponent<EnemyBehaviour>().enabled = false;
            Instantiate(_stunParticle, enemy.transform);
        }
        yield return new WaitForSeconds(_debuffData.duration);
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.rb.constraints = RigidbodyConstraints2D.None;
            enemy.GetComponent<EnemyBehaviour>().enabled = true;
            Destroy(enemy.transform.Find("StunDebuff(Clone)").gameObject);
        }
    }

    private void StartSlow()
    {
        ResetCooldown();
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.enemyData.chaseSpeed *= _debuffData.slowAmount;
            enemy.enemyData.attackSpeed *= _debuffData.slowAmount;
            enemy.enemyData.moveSpeed *= _debuffData.slowAmount;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                foreach (RangedData rangedData in rangedComponent.rangeds)
                {
                    rangedData.speed *= _debuffData.slowAmount;
                }
                //enemy.GetComponentInChildren<RangedComponent>().spell.rangedData.speed *= 
               // enemy.GetComponentInChildren<RangedComponent>().bullet.rangedData.speed *= _debuffData.slowAmount;
            }
            Instantiate(_slowParticle, enemy.transform);
            Debug.Log("SLOW");
        }
    }

    private void StopSlow()
    {
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.enemyData.chaseSpeed /= _debuffData.slowAmount;
            enemy.enemyData.attackSpeed /= _debuffData.slowAmount;
            enemy.enemyData.moveSpeed /= _debuffData.slowAmount;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                foreach (RangedData rangedData in rangedComponent.rangeds)
                {
                    rangedData.speed /= _debuffData.slowAmount;
                }
                //enemy.GetComponentInChildren<RangedComponent>().spell.rangedData.speed /= _debuffData.slowAmount;
                //enemy.GetComponentInChildren<RangedComponent>().bullet.rangedData.speed /= _debuffData.slowAmount;
            }
            Debug.Log("STOP SLOW");
            Destroy(enemy.transform.Find("SlowDebuff(Clone)").gameObject);
        }
    }



    private IEnumerator Slow()
    {
        StartSlow();
        yield return new WaitForSeconds(_debuffData.duration);
        StopSlow();
    }

    public override void ChangeSkill(string skillName)
    {
        base.ChangeSkill(skillName);
        this._debuffData = (DebuffData)_skillData;
    }

}