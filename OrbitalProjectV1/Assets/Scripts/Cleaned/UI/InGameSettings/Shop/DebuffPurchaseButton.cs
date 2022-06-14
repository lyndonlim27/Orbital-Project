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

    public void PurchaseDebuffSkill()
    {
        shop.AddDebuffSkill(_debuffData.skillName);
    }

    private void Initialise()
    {

        _skillTitle = _debuffData.skillName.Substring(0, 4) + " (Level " +
            _debuffData.skillName.Substring(4, 1) + ")";

        image.overrideSprite = _debuffData.sprite;

        textDisplay.text = _skillTitle + "\n";

        if (_skillTitle.Contains("Slow"))
        {
            textDisplay.text += "Slow: - " + _debuffData.slowAmount + " MS" + "\n";
        }

        textDisplay.text += "Duration: + " + _debuffData.duration + " SEC" + "\n" +
            "Cooldown: " + _debuffData.cooldown + " sec" + "\n" +
            "Mana Cost: " + _debuffData.manaCost + " MP" + "\n" +
            "Cost: " + _debuffData.goldCost + " gold";
    }

    public string GetDebuffName()
    {
        return _debuffData.skillName;
    }
}
