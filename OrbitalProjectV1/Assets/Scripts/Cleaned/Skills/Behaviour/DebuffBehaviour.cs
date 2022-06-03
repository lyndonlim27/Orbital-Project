using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffBehaviour : SkillBehaviour
{
    private DebuffData _debuffData;
    private bool _stop;
    public override void ActivateSkill()
    {
        if (_debuffData != null && CheckEnoughMana())
        {
            _player.UseMana(_debuffData.manaCost);
            switch (_debuffData.debuffType)
            {
                default:
                case DebuffData.DEBUFF_TYPE.STUN:
                    StartCoroutine(Stun());
                    break;
                case DebuffData.DEBUFF_TYPE.SLOW:
                    StartCoroutine(Slow());
                    debuffAnimator.SetTrigger("Activate");
                    break;
            }
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _debuffData = (DebuffData)skillData;

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
        }
        yield return new WaitForSeconds(_debuffData.duration);
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.rb.constraints = RigidbodyConstraints2D.None;
            enemy.GetComponent<EnemyBehaviour>().enabled = true;
        }
    }

    private void StartSlow()
    {
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.enemyData.chaseSpeed *= _debuffData.slowAmount;
            enemy.enemyData.attackSpeed *= _debuffData.slowAmount;
            enemy.enemyData.moveSpeed *= _debuffData.slowAmount;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                enemy.GetComponentInChildren<RangedComponent>().spell.rangedData.speed *= _debuffData.slowAmount;
                enemy.GetComponentInChildren<RangedComponent>().bullet.rangedData.speed *= _debuffData.slowAmount;
            }
            Debug.Log("SLOW");
        }
    }

    private void StopSlow()
    {
        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
        {
            enemy.enemyData.chaseSpeed /= 0.5f;
            enemy.enemyData.attackSpeed /= 0.5f;
            enemy.enemyData.moveSpeed /= 0.5f;
            RangedComponent rangedComponent = enemy.GetComponentInChildren<RangedComponent>();
            if (rangedComponent != null)
            {
                enemy.GetComponentInChildren<RangedComponent>().spell.rangedData.speed /= _debuffData.slowAmount;
                enemy.GetComponentInChildren<RangedComponent>().bullet.rangedData.speed /= _debuffData.slowAmount;
            }
            Debug.Log("STOP SLOW");
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
        this._debuffData = (DebuffData)skillData;
    }

}