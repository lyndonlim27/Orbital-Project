using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillBehaviour : SkillBehaviour
{

    public AttackData _attackData { get; private set; }
    private Animator _attackAnimator;
    private Rigidbody2D _playerRb;
    private TrailRenderer _playerTr;
    private RuntimeAnimatorController _dashAttackVFX;
    private RuntimeAnimatorController _shockwaveAttackVFX;
    private Collider2D _collider;
    private SoundEffect _soundEffect;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _attackData = _player.GetAttackData();
        _skillData = _attackData;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _playerTr = _player.GetComponent<TrailRenderer>();
        _attackAnimator = GameObject.Find("AttackSkillAnimator").GetComponent<Animator>();
        _dashAttackVFX = Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/DashAttackVFX");
        _shockwaveAttackVFX = Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/ShockwaveAttackVFX");
        _collider = _attackAnimator.GetComponent<Collider2D>();
        _soundEffect = _attackAnimator.GetComponent<SoundEffect>();
        SetData();
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
                case AttackData.ATTACK_TYPE.DASH:
                    _attackAnimator.runtimeAnimatorController = _dashAttackVFX;
                    StartCoroutine(Dash());
                    break;
                case AttackData.ATTACK_TYPE.SHOCKWAVE:
                    _attackAnimator.runtimeAnimatorController = _shockwaveAttackVFX;
                    Shockwave();
                    break;
            }
            ResetCooldown();

        }
    }
    private void Fire()
    {
        Debug.Log("FIRE");
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
        //fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        fire1.GetComponent<Rigidbody2D>().AddForce(direction1, ForceMode2D.Impulse);
        fire2.GetComponent<Rigidbody2D>().AddForce(direction2, ForceMode2D.Impulse);
        fire3.GetComponent<Rigidbody2D>().AddForce(direction3, ForceMode2D.Impulse);
        //fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
        //fire3.GetComponent<Rigidbody2D>().velocity = direction3 * 3;
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
        fire1.GetComponent<Rigidbody2D>().AddForce(direction1, ForceMode2D.Impulse);
        fire2.GetComponent<Rigidbody2D>().AddForce(direction2, ForceMode2D.Impulse);
        fire3.GetComponent<Rigidbody2D>().AddForce(direction3, ForceMode2D.Impulse);

        //fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        //fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
        //fire3.GetComponent<Rigidbody2D>().velocity = direction3 * 3;
    }

    private void FireAttack3()
    {
        FireAttack1();
        FireAttack2();
        Vector3 direction1 = new Vector3(1, 0, 0);
        Vector3 direction2 = new Vector3(-1, 0, 0);
        GameObject fire1 = Instantiate(_attackData.prefab, _player.transform.position + direction1 * 1, Quaternion.Euler(0, 0, 0));
        GameObject fire2 = Instantiate(_attackData.prefab, _player.transform.position + direction2 * 1, Quaternion.Euler(0, 0, 180));
        //fire1.GetComponent<Rigidbody2D>().velocity = direction1 * 3;
        //fire2.GetComponent<Rigidbody2D>().velocity = direction2 * 3;
        fire1.GetComponent<Rigidbody2D>().AddForce(direction1, ForceMode2D.Impulse);
        fire2.GetComponent<Rigidbody2D>().AddForce(direction2, ForceMode2D.Impulse);
        //fire3.GetComponent<Rigidbody2D>().AddForce(direction3, ForceMode2D.Impulse);
    }


    private void Shuriken()
    {
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
        _player.SetAttackData(this._attackData);
    }

    private IEnumerator Dash()
    {
        Vector2 dir = _player.GetDirection();
        if (dir == new Vector2(0, 0))
        {
            dir = new Vector2(0, -1);
        }
        //_playerRb.velocity = dir * 10;
        _soundEffect.AudioClip1();
        yield return new WaitForSeconds(0.1f);
        _playerRb.AddForce(dir * 10000);
        _playerTr.emitting = true;
        _collider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        _playerTr.emitting = false;
        yield return new WaitForSeconds(0.1f);
        _attackAnimator.SetTrigger("Trigger");
        _collider.enabled = false;
    }

    private void Shockwave()
    {
        _attackAnimator.SetTrigger("Trigger" + _attackData.numOfProjectiles);
    }
}
