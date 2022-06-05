using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBehaviour : SkillBehaviour
{
    private BuffData _buffData;
    private Animator _buffAnimator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _buffAnimator = GameObject.Find("BuffAnimator").GetComponent<Animator>();
        _buffData = (BuffData)_skillData;
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
            }
        }
    }

    private void Heal()
    {
        ResetCooldown();
        _player.AddHealth(_buffData.healAmount);
    }

    private IEnumerator Speed()
    {
        ResetCooldown();
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
}
