using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackPurchaseButton : SkillPurchaseButton
{
    [SerializeField] private AttackData _attackData;
    private string _skillTitle;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialise()
    {
        switch (_attackData.skillName)
        {
            default:
            case "Fireball1Data":
                if (!player.GetPlayerData().ranged)
                {
                    _attackData = Resources.Load<AttackData>("Data/SkillData/Speed1Data");
                    _skillTitle = "Dagger Spin  (Level 1)";
                }
                else
                {
                    _skillTitle = "Fireball (Level 1)";
                }
                break;
            case "Fireball2Data":
                if (!player.GetPlayerData().ranged)
                {
                    _attackData = Resources.Load<AttackData>("Data/SkillData/Speed2Data");
                    _skillTitle = "Dagger Spin (Level 2)";
                }
                else
                {
                    _skillTitle = "Fireball (Level 2)";
                }
                break;
            case "Fireball3Data":
                if (!player.GetPlayerData().ranged)
                {
                    _attackData = Resources.Load<AttackData>("Data/SkillData/Speed3Data");
                    _skillTitle = "Dagger Spin (Level 3)";
                }
                else
                {
                    _skillTitle = "Fireball (Level 3)";
                }
                break;
        }

        GetComponent<Image>().overrideSprite = _attackData.sprite;
        TextMeshProUGUI textDisplay = GetComponentInChildren<TextMeshProUGUI>(true);

        if (_skillTitle.Contains("Fireball"))
        {
            textDisplay.text = _skillTitle + "\n" +
                "Damage: - " + _attackData.damage + " HP" + "\n" +
                "Fireballs + " + _attackData.numOfProjectiles + "\n" +
                "Cooldown: " + _attackData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _attackData.manaCost + " MP" + "\n" +
                "Cost: " + _attackData.goldCost + " gold" + "\n" +
                "-Shoots out " + _attackData.numOfProjectiles + " fireballs-";
        }
        else if (_skillTitle.Contains("Dagger"))
        {
            textDisplay.text = _skillTitle + "\n" +
                "Damage: + " + _attackData.damage + " HP" + "\n" +
                "Fireballs + " + _attackData.numOfProjectiles + "\n" +
                "Cooldown: " + _attackData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _attackData.manaCost + " MP" + "\n" +
                "Cost: " + _attackData.goldCost + " gold" + "\n" +
                "-Spins " + _attackData.numOfProjectiles + " daggers around-";
        }


    }

    public void PurchaseAttackSkill()
    {
        shop.AddAttackSkill(_attackData.skillName);
    }
}
