using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBehaviour : SkillBehaviour
{
    private BuffData _buffData;
    private Animator _buffAnimator;
    private WeaponPickup weaponManager;
    public bool inStealth;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _buffAnimator = GameObject.Find("BuffAnimator").GetComponent<Animator>();
        _buffData = (BuffData)_skillData;
        inStealth = false;
        weaponManager = FindObjectOfType<WeaponPickup>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ActivateSkill()
    {
        if (_buffData != null && CanCast())
        {
            _player.UseMana(_buffData.manaCost);
            switch (_buffData.buffType)
            {
                default:
                case BuffData.BUFF_TYPE.HEAL:
                    _buffAnimator.runtimeAnimatorController =
                        Resources.Load<AnimatorOverrideController>("Animations/AnimatorControllers/HealBuffVFX");
                    _buffAnimator.SetTrigger("Activate");
                    Heal();
                    break;
                case BuffData.BUFF_TYPE.SPEED:
                    _buffAnimator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/SpeedBuffVFX");
                    _buffAnimator.SetBool("Activate", true);
                    StartCoroutine(Speed());
                    break;
                case BuffData.BUFF_TYPE.STEALTH:
                    _buffAnimator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/StealthBuffVFX");
                    _buffAnimator.SetTrigger("Activate");
                    StartCoroutine(Stealth());
                    break;
                case BuffData.BUFF_TYPE.INVULNERABLE:
                    _buffAnimator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animations/AnimatorControllers/InvulnerableBuffVFX");
                    _buffAnimator.SetBool("Activate", true);
                    StartCoroutine(Invulnerable());
                    break;
            }
            ResetCooldown();
        }
    }

    private void Heal()
    {
        _player.AddHealth(_buffData.healAmount);
    }

    private IEnumerator Speed()
    {
        _player.SetSpeed(_buffData.speedAmount);
        yield return new WaitForSeconds(_buffData.duration);
        _buffAnimator.SetBool("Activate", false);
        _player.SetSpeed(-_buffData.speedAmount);
    }

    public override void ChangeSkill(string skillName)
    {
        base.ChangeSkill(skillName);
        this._buffData = (BuffData)_skillData;
    }

    private IEnumerator Stealth()
    {
        yield return new WaitForSeconds(0.3f);
        _player.tag = "Stealth";
        inStealth = true;
        _player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 80);
        weaponManager.ActiveWeapon().GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 80);
        yield return new WaitForSeconds(_buffData.duration);
        StartCoroutine(Unstealth());
    }

    public IEnumerator Unstealth()
    {
        _buffAnimator.SetTrigger("Activate");
        yield return new WaitForSeconds(0.3f);
        _player.tag = "Player";
        _player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        weaponManager.ActiveWeapon().GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        inStealth = false;
    }

    private IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(1.583f);
        Debug.Log("YAS");
        _player.SetInvulnerability(true);
        yield return new WaitForSeconds(_buffData.duration);
        _player.SetInvulnerability(false);
        _buffAnimator.SetBool("Activate", false);
    }
}
