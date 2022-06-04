using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class SkillBehaviour : MonoBehaviour
{
    [SerializeField] protected SkillData skillData;
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TextMeshProUGUI textCooldown;
    protected Player _player;
    protected float currCooldown;
    protected Animator debuffAnimator;
    public abstract void ActivateSkill();
    public virtual void ChangeSkill(string skillName)
    {
        this.skillData = Resources.Load<SkillData>("Data/SkillData/" + skillName);
        GetComponent<Image>().overrideSprite = skillData.sprite;
    }

    public virtual void Start()
    {
        currCooldown = 0;
        _player = FindObjectOfType<Player>();
        GetComponent<Image>().overrideSprite = skillData.sprite;
        debuffAnimator = GameObject.Find("DebuffAnimator").GetComponent<Animator>();
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0;
    }

    public virtual void Update()
    {
        if(currCooldown > 0)
        {
            //imageCooldown.fillAmount = currCooldown;
            textCooldown.text = Mathf.RoundToInt(currCooldown).ToString();
            textCooldown.gameObject.SetActive(true);
            Tick();
        }

        if(currCooldown < 0)
        {
            textCooldown.gameObject.SetActive(false);
            //imageCooldown.fillAmount = 0;
            currCooldown = 0;
        }

        imageCooldown.fillAmount = currCooldown/skillData.cooldown;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSkill("SlowData");
        }
    }

    public bool CanCast()
    {
        return  (_player.GetMana() >= skillData.manaCost) &&
            (currCooldown == 0);
    }


    public void ResetCooldown()
    {
        currCooldown = skillData.cooldown;
    }

    private void Tick()
    {
        currCooldown -= Time.deltaTime;
    }
}
