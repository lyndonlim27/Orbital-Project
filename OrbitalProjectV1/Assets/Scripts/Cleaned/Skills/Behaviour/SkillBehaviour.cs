using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class SkillBehaviour : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    [SerializeField] protected Image imageCooldown;
    [SerializeField] protected TextMeshProUGUI textCooldown;
    [SerializeField] protected TextMeshProUGUI manaCostText;
    private string promptText;
    protected Player _player;
    protected float currCooldown;
    private AudioSource _audioSource;
    public abstract void ActivateSkill();

    public virtual void Start()
    {
        currCooldown = 0;
        _player = FindObjectOfType<Player>();
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0;
        promptText = "";
        _audioSource = GetComponent<AudioSource>();
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
    }

    protected void SetData()
    {
        if (_skillData != null)
        {
            manaCostText.gameObject.SetActive(true);
            manaCostText.text = _skillData.manaCost.ToString();
            GetComponent<Image>().overrideSprite = _skillData.sprite;
        }
    }


    public virtual void ChangeSkill(string skillName)
    {
        this._skillData = Resources.Load<SkillData>("Data/SkillData/" + skillName);
        Debug.Log("Image is : " + GetComponent<Image>());
        //GetComponent<Image>().overrideSprite = _skillData.sprite;
        GetComponent<Image>().sprite = _skillData.sprite;

        manaCostText.text = _skillData.manaCost.ToString();
        manaCostText.gameObject.SetActive(true);
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
            _audioSource.Play();
        }
        else if(currCooldown > 0)
        {
            promptText = "Skill on cooldown";
            _audioSource.Play();
        }
        else if(_player.currMana < _skillData.manaCost)
        {
            promptText = "Not enough mana";
            _audioSource.Play();
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
