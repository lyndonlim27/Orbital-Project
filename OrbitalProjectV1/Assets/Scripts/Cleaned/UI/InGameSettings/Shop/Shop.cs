using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Image _promptImage;
    private TextMeshProUGUI _promptText;
    private DebuffBehaviour _debuffSkill;
    private BuffBehaviour _buffSkill;
    private bool Unlocked;
    private Player _player;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _debuffSkill = FindObjectOfType<DebuffBehaviour>();
        _buffSkill = FindObjectOfType<BuffBehaviour>();
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
        else if (_debuffSkill.GetSkillData() != null && _debuffSkill.GetSkillData().goldCost > _player.currGold)
        {
            _promptText.text = "Not enough gold";
        }
        else
        {
            _promptText.text = "Skill purchased";
            _debuffSkill.ChangeSkill(debuffSkillName);
            _player.UseGold(Resources.Load<SkillData>("Data/SkillData/" + debuffSkillName).goldCost);
            _audioSource.Play();
        }
        StartCoroutine(UpdateText());
    }

    public void AddBuffSkill(string buffSkillName)
    {
        if (_buffSkill.GetSkillData() != null && buffSkillName == _buffSkill.GetSkillData().skillName)
        {
            _promptText.text = "Skill has already been purchased";
        }
        else if (_buffSkill.GetSkillData() != null && _buffSkill.GetSkillData().goldCost > _player.currGold)
        {
            _promptText.text = "Not enough gold";
        }
        else
        {
            _promptText.text = "Skill purchased";
            _buffSkill.ChangeSkill(buffSkillName);
            _player.UseGold(Resources.Load<SkillData>("Data/SkillData/" + buffSkillName).goldCost);
            _audioSource.Play();
        }
        StartCoroutine(UpdateText());
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

}
