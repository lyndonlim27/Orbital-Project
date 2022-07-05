using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MenuBehaviour
{
    [SerializeField] private Image _promptImage;
    private TextMeshProUGUI _promptText;
    private DebuffBehaviour _debuffSkill;
    private BuffBehaviour _buffSkill;
    private AttackSkillBehaviour _attackSkill;
    private bool Unlocked;
    private Player _player;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    protected override void Start()
    {
        _debuffSkill = FindObjectOfType<DebuffBehaviour>(true);
        _buffSkill = FindObjectOfType<BuffBehaviour>(true);
        _attackSkill = FindObjectOfType<AttackSkillBehaviour>(true);
        _player = FindObjectOfType<Player>();
        _promptText = _promptImage.GetComponentInChildren<TextMeshProUGUI>();
        _promptText.text = "";
        _promptImage.gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** 
     * Player Upgrades.
     */
    private void AddHealth()
    {
        
    }


    private void AddSpeed()
    {
       
    }

    public void AddDebuffSkill(string debuffSkillName)
    {
        if (_debuffSkill.GetSkillData() != null && debuffSkillName == _debuffSkill.GetSkillData().skillName)
        {
            _promptText.text = "Skill has already been purchased";
        }
        else if (Resources.Load<SkillData>("Data/SkillData/" + debuffSkillName).goldCost > _player.currGold)
        {
            _promptText.text = "Not enough gold";
        }
        else
        {
            _promptText.text = "Skill purchased";
            _player.UseGold(Resources.Load<SkillData>("Data/SkillData/" + debuffSkillName).goldCost);
            _audioSource.Play();
            SetDebuffButtons(debuffSkillName);
        }
        StartCoroutine(UpdateText());
    }

    public void SetDebuffButtons(string debuffSkillName)
    {
        _debuffSkill.ChangeSkill(debuffSkillName);
        foreach (DebuffPurchaseButton debuff in GetComponentsInChildren<DebuffPurchaseButton>())
        {
            if (debuffSkillName.Contains("Slow"))
            {
                if (debuff.GetDebuffName().Contains("Stun"))
                {
                    debuff.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    debuff.GetComponent<Button>().enabled = false;
                }
            }
            else
            {
                if (debuff.GetDebuffName().Contains("Slow"))
                {
                    debuff.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    debuff.GetComponent<Button>().enabled = false;
                }
            }
        }
    }

    public void AddBuffSkill(string buffSkillName)
    {
        if (_buffSkill.GetSkillData() != null && buffSkillName == _buffSkill.GetSkillData().skillName)
        {
            _promptText.text = "Skill has already been purchased";
        }
        else if (Resources.Load<SkillData>("Data/SkillData/" + buffSkillName).goldCost > _player.currGold)
        {
            _promptText.text = "Not enough gold";
        }
        else
        {
            _promptText.text = "Skill purchased";
            _player.UseGold(Resources.Load<SkillData>("Data/SkillData/" + buffSkillName).goldCost);
            _audioSource.Play();
            SetBuffButtons(buffSkillName);
        }
        StartCoroutine(UpdateText());
    }

    public void SetBuffButtons(string buffSkillName)
    {
        _buffSkill.ChangeSkill(buffSkillName);
        foreach (BuffPurchaseButton buff in GetComponentsInChildren<BuffPurchaseButton>())
        {
            if (buffSkillName.Contains("Heal") || buffSkillName.Contains("Speed"))
            {
                if (buff.GetBuffName().Contains("Invulnerable") || buff.GetBuffName().Contains("Stealth"))
                {
                    buff.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    buff.GetComponent<Button>().enabled = false;
                }
            }
            else
            {
                if (buff.GetBuffName().Contains("Heal") || buff.GetBuffName().Contains("Speed"))
                {
                    buff.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    buff.GetComponent<Button>().enabled = false;
                }
            }
        }
    }

    public void AddAttackSkill(string attackSkillName)
    {
        if (_attackSkill.GetSkillData() != null && attackSkillName == _attackSkill.GetSkillData().skillName)
        {
            _promptText.text = "Skill has already been purchased";
        }
        else if (Resources.Load<SkillData>("Data/SkillData/" + attackSkillName).goldCost > _player.currGold)
        {
            _promptText.text = "Not enough gold";
        }
        else
        {
            _promptText.text = "Skill purchased";
            _player.UseGold(Resources.Load<SkillData>("Data/SkillData/" + attackSkillName).goldCost);
            _audioSource.Play();
            SetAttackButtons(attackSkillName);
        }
        StartCoroutine(UpdateText());
    }

    public void SetAttackButtons(string attackSkillName)
    {
        _attackSkill.ChangeSkill(attackSkillName);
        foreach (AttackPurchaseButton attack in GetComponentsInChildren<AttackPurchaseButton>())
        {
            if (attackSkillName.Contains("Fireball") || attackSkillName.Contains("Shuriken"))
            {
                if (attack.GetAttackName().Contains("Shockwave") || attack.GetAttackName().Contains("Dash"))
                {
                    attack.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    attack.GetComponent<Button>().enabled = false;
                }
            }
            else
            {
                if (attack.GetAttackName().Contains("Fireball") || attack.GetAttackName().Contains("Shuriken"))
                {
                    attack.GetComponent<Image>().color = new Color32(96, 96, 96, 255);
                    attack.GetComponent<Button>().enabled = false;
                }
            }
        }
    }

    private void AddMana()
    {

    }

    private IEnumerator UpdateText()
    {
        _promptImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _promptImage.gameObject.SetActive(false);
    }


    public override void Active()
    {
        base.Active();
        _promptImage.gameObject.SetActive(false);
    }
}
