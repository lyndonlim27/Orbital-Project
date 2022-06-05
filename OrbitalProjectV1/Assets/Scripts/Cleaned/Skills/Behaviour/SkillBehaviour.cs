using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class SkillBehaviour : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TextMeshProUGUI textCooldown;
    [SerializeField] private TextMeshProUGUI manaCostText;
    private string promptText;
    protected Player _player;
    protected float currCooldown;
    protected Animator debuffAnimator;
    public abstract void ActivateSkill();


    public virtual void Start()
    {
        currCooldown = 0;
        _player = FindObjectOfType<Player>();
        debuffAnimator = GameObject.Find("DebuffAnimator").GetComponent<Animator>();
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0;
        promptText = "";
        if(_skillData != null)
        {
            manaCostText.gameObject.SetActive(true);
            manaCostText.text = _skillData.manaCost.ToString();
            GetComponent<Image>().overrideSprite = _skillData.sprite;
        }

    }

    public virtual void Update()
    {
        if (_skillData != null)
        {
            if (currCooldown > 0)
            {
                //imageCooldown.fillAmount = currCooldown;
                textCooldown.text = Mathf.RoundToInt(currCooldown).ToString();
                textCooldown.gameObject.SetActive(true);
                Tick();
            }

            if (currCooldown < 0)
            {
                textCooldown.gameObject.SetActive(false);
                //imageCooldown.fillAmount = 0;
                currCooldown = 0;
            }

            imageCooldown.fillAmount = currCooldown / _skillData.cooldown;
        }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeSkill("SlowData");
            }
    }

    public virtual void ChangeSkill(string skillName)
    {
        this._skillData = Resources.Load<SkillData>("Data/SkillData/" + skillName);
        GetComponent<Image>().overrideSprite = _skillData.sprite;
        manaCostText.text = _skillData.manaCost.ToString();
    }

    public bool CanCast()
    {
        return  (_player.currMana >= _skillData.manaCost) &&
            (currCooldown == 0);
    }


    public void ResetCooldown()
    {
        currCooldown = _skillData.cooldown;
    }

    private void Tick()
    {
        currCooldown -= Time.deltaTime;
    }

    public string PromptCheck()
    {
        if(_skillData == null)
        {
            promptText = "Skill not unlocked";
        }
        else if(currCooldown > 0)
        {
            promptText = "Skill on cooldown";
        }
        else if(_player.currMana < _skillData.manaCost)
        {
            promptText = "Not enough mana";
        }
        else
        {
            promptText = "";
        }
        return promptText;
    }

    public SkillData GetSkillData()
    {
        return _skillData;
    }
}
