using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillBehaviour : SkillBehaviour
{

    public AttackData _attackData { get; private set; }
    private Animator _attackAnimator;
    private GameObject _lightningPrefab;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _attackData = (AttackData)_skillData;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ActivateSkill()
    {
        if (_attackData != null && CanCast())
        {
            _player.UseMana(_attackData.manaCost);
            switch (_attackData.attackType)
            {
                default:
                case AttackData.ATTACK_TYPE.FIRE:
                    Fire();
                    break;

            }


        }
    }
    private void Fire()
    {
        Debug.Log("FIRE");
        ResetCooldown();
        _lightningPrefab = Resources.Load<GameObject>("SkillPrefab/Fireball");
        if (_attackData.numOfProjectiles == 3)
        {
            FireAttack1();
        }
        else if(_attackData.numOfProjectiles == 6)
        {
            FireAttack2();
        }
        else
        {
            FireAttack3();
        }
    }

    private void FireAttack1()
    {
        Vector3 direction1 = new Vector3(0, 1, 0);
        Vector3 direction2 = new Vector3(-1, -1, 0);
        Vector3 direction3 = new Vector3(1, -1, 0);
        GameObject fire1 = Instantiate(_lightningPrefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 90));
        GameObject fire2 = Instantiate(_lightningPrefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, -135));
        GameObject fire3 = Instantiate(_lightningPrefab, _player.transform.position + direction3 * 1, Quaternion.Euler(0, 0, -45));
        fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
        fire3.GetComponent<Rigidbody2D>().velocity = direction3 * 3;
    }

    private void FireAttack2()
    {
        FireAttack1();
        Vector3 direction1 = new Vector3(0, -1, 0);
        Vector3 direction2 = new Vector3(-1, 1, 0);
        Vector3 direction3 = new Vector3(1, 1, 0);
        GameObject fire1 = Instantiate(_lightningPrefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 270));
        GameObject fire2 = Instantiate(_lightningPrefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 135));
        GameObject fire3 = Instantiate(_lightningPrefab, _player.transform.position + direction3 * 1, Quaternion.Euler(0, 0, 45));
        fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
        fire3.GetComponent<Rigidbody2D>().velocity = direction3 * 3;
    }

    private void FireAttack3()
    {
        FireAttack1();
        FireAttack2();
        Vector3 direction1 = new Vector3(1, 0, 0);
        Vector3 direction2 = new Vector3(-1, 0, 0);
        GameObject fire1 = Instantiate(_lightningPrefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject fire2 = Instantiate(_lightningPrefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 180));
        fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
    }
    public override void ChangeSkill(string skillName)
    {
        base.ChangeSkill(skillName);
        this._attackData = (AttackData)_skillData;
    }
}
