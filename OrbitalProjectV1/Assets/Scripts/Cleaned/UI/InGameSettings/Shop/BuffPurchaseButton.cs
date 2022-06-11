using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuffPurchaseButton : SkillPurchaseButton
{
    [SerializeField] private BuffData _buffData;
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
        switch (_buffData.skillName)
        {
            default:
            case "Heal1Data":
                if (!player.GetPlayerData().ranged)
                {
                    _buffData = Resources.Load<BuffData>("Data/SkillData/Speed1Data");
                    _skillTitle = "Speed (Level 1)";
                }
                else
                {
                    _skillTitle = "Heal (Level 1)";
                }
                break;
            case "Heal2Data":
                if (!player.GetPlayerData().ranged)
                {
                    _buffData = Resources.Load<BuffData>("Data/SkillData/Speed2Data");
                    _skillTitle = "Speed (Level 2)";
                }
                else
                {
                    _skillTitle = "Heal (Level 2)";
                }
                break;
            case "Heal3Data":
                if (!player.GetPlayerData().ranged)
                {
                    _buffData = Resources.Load<BuffData>("Data/SkillData/Speed3Data");
                    _skillTitle = "Speed (Level 3)";
                }
                else
                {
                    _skillTitle = "Heal (Level 3)";
                }
                break;
        }




        GetComponent<Image>().overrideSprite = _buffData.sprite;
        TextMeshProUGUI textDisplay = GetComponentInChildren<TextMeshProUGUI>(true);

        if (_skillTitle.Contains("Heal"))
        {
            textDisplay.text = _skillTitle + "\n" +
                "Heal: + " + _buffData.healAmount + " HP" + "\n" +
                "Cooldown: " + _buffData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _buffData.manaCost + " MP" + "\n" +
                "Cost: " + _buffData.goldCost + " gold";
        }
        else if (_skillTitle.Contains("Speed"))
        {
            textDisplay.text = _skillTitle + "\n" +
                "Speed: + " + _buffData.speedAmount + " ms" + "\n" +
                "Duration: " + _buffData.duration + " sec" + "\n" +
                "Cooldown: " + _buffData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _buffData.manaCost + " MP" + "\n" +
                "Cost: " + _buffData.goldCost + " gold";
        }

            
    }

    public void PurchaseBuffSkill()
    {
        Debug.Log(_buffData.skillName);
        shop.AddBuffSkill(_buffData.skillName);
    }
}