using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffBehaviour : SkillBehaviour
{
    private DebuffData _debuffData;
    private Animator _debuffAnimator;
    private AnimatorOverrideController _stunDebuffVFX;
    private RuntimeAnimatorController _slowDebuffVFX;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _debuffAnimator = GameObject.Find("DebuffAnimator").GetComponent<Animator>();
        _debuffData = _player.GetDebuffData();
        _skillData = _debuffData;
        _stunDebuffVFX = Resources.Load<AnimatorOverrideController>("Animations/AnimatorControllers/StunDebuffVFX");
        _slowDebuffVFX = Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/SlowDebuffVFX");
        SetData();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ActivateSkill()
    {
        if (_debuffData != null && CanCast())
        {
            _player.UseMana(_debuffData.manaCost);
            switch (_debuffData.debuffType)
            {
                default:
                case DebuffData.DEBUFF_TYPE.STUN:
                    _debuffAnimator.runtimeAnimatorController = _stunDebuffVFX;
                    _debuffAnimator.SetTrigger("Activate");
                    StartCoroutine(Stun());
                    break;
                case DebuffData.DEBUFF_TYPE.SLOW:
                    _debuffAnimator.runtimeAnimatorController = _slowDebuffVFX;
                    _debuffAnimator.SetTrigger("Activate");
                    StartCoroutine(Slow());
                    break;
            }
        }
    }


    private IEnumerator Stun()
    {
        ResetCooldown();
        foreach(EnemyBehaviour enemy in _player.GetCurrentRoom().GetComponentsInChildren<EnemyBehaviour>())
        {
            //enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            enemy.Debuffed = true;
            enemy.Freeze();
            //enemy.GetComponent<EnemyBehaviour>().enabled = false;
            Instantiate(_debuffData.particleSystem, enemy.transform);
        }
        yield return new WaitForSeconds(_debuffData.duration);
        foreach (EnemyBehaviour enemy in _player.GetCurrentRoom().GetComponentsInChildren<EnemyBehaviour>())
        {
            enemy.Debuffed = false;
            enemy.UnFreeze();
            Destroy(enemy.transform.Find("StunDebuff(Clone)").gameObject);
        }
    }
    
    private void StartSlow()
    {
        ResetCooldown();
        foreach (EnemyBehaviour enemy in _player.GetCurrentRoom().GetComponentsInChildren<EnemyBehaviour>())
        {
            enemy.enemyData.chaseSpeed *= _debuffData.slowAmount;
            enemy.enemyData.attackSpeed *= _debuffData.slowAmount;
            enemy.enemyData.moveSpeed *= _debuffData.slowAmount;
            enemy.Debuffed = true;
            enemy.animator.speed *= _debuffData.slowAmount;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                foreach (RangedData rangedData in rangedComponent.rangeds)
                {
                    rangedData.speed *= _debuffData.slowAmount;
                }
            }
            Instantiate(_debuffData.particleSystem, enemy.transform);
        }
    }

    private void StopSlow()
    {
        foreach (EnemyBehaviour enemy in _player.GetCurrentRoom().GetComponentsInChildren<EnemyBehaviour>())
        {
            enemy.Debuffed = false;
            enemy.enemyData.chaseSpeed /= _debuffData.slowAmount;
            enemy.enemyData.attackSpeed /= _debuffData.slowAmount;
            enemy.enemyData.moveSpeed /= _debuffData.slowAmount;
            enemy.animator.speed /= _debuffData.slowAmount;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                foreach (RangedData rangedData in rangedComponent.rangeds)
                {
                    rangedData.speed /= _debuffData.slowAmount;
                }
            }
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
        _player.SetDebuffData(this._debuffData);
    }
}