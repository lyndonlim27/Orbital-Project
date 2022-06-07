using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DebuffPurchaseButton : SkillPurchaseButton
{
    [SerializeField] private DebuffData _debuffData;
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

    public void PurchaseDebuffSkill()
    {
        shop.AddDebuffSkill(_debuffData.skillName);
    }

    private void Initialise()
    {
        switch (_debuffData.skillName)
        {
            default:
            case "Stun1Data":
                _skillTitle = "Stun (Level 1)";
                break;
            case "Stun2Data":
                _skillTitle = "Stun (Level 2)";
                break;
            case "Stun3Data":
                _skillTitle = "Stun (Level 3)";
                break;
            case "Slow1Data":
                _skillTitle = "Slow (Level 1)";
                break;
            case "Slow2Data":
                _skillTitle = "Slow (Level 2)";
                break;
            case "Slow3Data":
                _skillTitle = "Slow (Level 3)";
                break;
        }
        _debuffData = Resources.Load<DebuffData>("Data/SkillData/" + _debuffData.skillName);
        GetComponent<Image>().overrideSprite = _debuffData.sprite;
        TextMeshProUGUI textDisplay = GetComponentInChildren<TextMeshProUGUI>(true);

        if (_skillTitle.Contains("Stun"))
        {
            textDisplay.text = _skillTitle + "\n" +
                "Duration: + " + _debuffData.duration + " SEC" + "\n" +
                "Cooldown: " + _debuffData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _debuffData.manaCost + " MP" + "\n" +
                "Cost: " + _debuffData.goldCost + " gold";
        }
        else
        {
            textDisplay.text = _skillTitle + "\n" +
                "Slow: - " +_debuffData.slowAmount + " MS" + "\n" +
                "Duration: + " + _debuffData.duration + " SEC" + "\n" +
                "Cooldown: " + _debuffData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _debuffData.manaCost + " MP" + "\n" +
                "Cost: " + _debuffData.goldCost + " gold";
        }
    }

    public string GetDebuffName()
    {
        return _debuffData.skillName;
    }



}
