using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillBehaviour : SkillBehaviour
{

    public AttackData _attackData { get; private set; }
    private Animator _attackAnimator;


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
                case AttackData.ATTACK_TYPE.SHURIKEN:
                    Shuriken();
                    break;
            }


        }
    }
    private void Fire()
    {
        Debug.Log("FIRE");
        ResetCooldown();
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
        GameObject fire1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 90));
        GameObject fire2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, -135));
        GameObject fire3 = Instantiate(_attackData.prefab, _player.transform.position + direction3 * 1, Quaternion.Euler(0, 0, -45));
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
        GameObject fire1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 270));
        GameObject fire2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 135));
        GameObject fire3 = Instantiate(_attackData.prefab, _player.transform.position + direction3 * 1, Quaternion.Euler(0, 0, 45));
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
        GameObject fire1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject fire2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 180));
        fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
    }


    private void Shuriken()
    {
        ResetCooldown();
        if (_attackData.numOfProjectiles == 2)
        {
            ShurikenAttack1();
        }
        else if (_attackData.numOfProjectiles == 3)
        {
            ShurikenAttack2();
        }
        else
        {
            ShurikenAttack3();
        }
    }

    private void ShurikenAttack1()
    {
        Vector3 direction1 = new Vector3(0, 1, 0);
        Vector3 direction2 = new Vector3(0, -1, 0);
        GameObject shuriken1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject shuriken2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 0));
        shuriken1.transform.SetParent(_player.transform);
        shuriken2.transform.SetParent(_player.transform);
        Destroy(shuriken1, _attackData.duration);
        Destroy(shuriken2, _attackData.duration);
    }

    private void ShurikenAttack2()
    {
        Vector3 direction1 = new Vector3(0, 1, 0);
        Vector3 direction2 = new Vector3(-1, -1, 0);
        Vector3 direction3 = new Vector3(1, -1, 0);
        GameObject shuriken1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject shuriken2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 0));
        GameObject shuriken3 = Instantiate(_attackData.prefab, _player.transform.position + direction3 * 1, Quaternion.Euler(0, 0, 0));
        shuriken1.transform.SetParent(_player.transform);
        shuriken2.transform.SetParent(_player.transform);
        shuriken3.transform.SetParent(_player.transform);
        Destroy(shuriken1, _attackData.duration);
        Destroy(shuriken2, _attackData.duration);
        Destroy(shuriken3, _attackData.duration);
    }


    private void ShurikenAttack3()
    {
        ShurikenAttack1();
        Vector3 direction1 = new Vector3(1, 0, 0);
        Vector3 direction2 = new Vector3(-1, 0, 0);
        GameObject shuriken1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject shuriken2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 0));
        shuriken1.transform.SetParent(_player.transform);
        shuriken2.transform.SetParent(_player.transform);
        Destroy(shuriken1, _attackData.duration);
        Destroy(shuriken2, _attackData.duration);
    }

    public override void ChangeSkill(string skillName)
    {
        base.ChangeSkill(skillName);
        this._attackData = (AttackData)_skillData;
    }
}
