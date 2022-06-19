using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuffPurchaseButton : SkillPurchaseButton
{
    [SerializeField] private BuffData _buffData;
    private string _skillTitle;
    private List<BuffData> _speedDatas;
    private List<BuffData> _stealthDatas;

    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log(player);
        if(player.GetBuffData() != null)
        {
            shop.SetBuffButtons(player.GetBuffData().skillName);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _speedDatas = new List<BuffData>();
        _stealthDatas = new List<BuffData>();
        for(int i = 1; i < 4; i++)
        {
            _speedDatas.Add(Resources.Load<BuffData>("Data/SkillData/Speed" + i + "Data"));
        }
        for (int i = 1; i < 4; i++)
        {
            _stealthDatas.Add(Resources.Load<BuffData>("Data/SkillData/Stealth" + i + "Data"));
        }
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialise()
    {
        if (!player.IsRanged())
        {
            if (_buffData.skillName.Contains("Heal"))
            {
                int level = int.Parse(_buffData.skillName.Substring(4, 1));
                _skillTitle = "Speed (Level " +  level + ")";
                _buffData = _speedDatas[level - 1];
            }
            else
            {
                int level = int.Parse(_buffData.skillName.Substring(12, 1));
                _skillTitle = "Stealth (Level " + level + ")";
                _buffData = _stealthDatas[level - 1];
            }
        }
        else
        {
            if (_buffData.skillName.Contains("Heal"))
            {
                _skillTitle = "Heal (Level " + _buffData.skillName.Substring(4, 1) + ")";
            }
            else
            {
                _skillTitle = "Invulnerable (Level " + _buffData.skillName.Substring(12, 1) + ")";
            }
                
        }

        image.overrideSprite = _buffData.sprite;
        textDisplay.text = _skillTitle + "\n";

        if (_skillTitle.Contains("Heal"))
        {
            textDisplay.text += "Heal: + " + _buffData.healAmount + " HP" + "\n";
        }
        else
        {
            if (_skillTitle.Contains("Speed"))
            {
                textDisplay.text += "Speed: + " + _buffData.speedAmount + " ms" + "\n";
            }
            textDisplay.text += "Duration: " + _buffData.duration + " sec" + "\n";
        }

        textDisplay.text += "Cooldown: " + _buffData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _buffData.manaCost + " MP" + "\n" +
                "Cost: " + _buffData.goldCost + " gold" + "\n";

        if (_skillTitle.Contains("Heal"))
        {
            textDisplay.text += "-Heal " + _buffData.healAmount + " Health-";
        }
        else if (_skillTitle.Contains("Speed"))
        {
            textDisplay.text += "_Increase speed by " + _buffData.speedAmount +
                " for " + _buffData.duration + " sec-";
        }
        else if (_skillTitle.Contains("Stealth"))
        {
            textDisplay.text += "-Becomes invisible for " + _buffData.duration +
                " sec until you attack or duration ended-";
        }
        else
        {
            textDisplay.text += "-Becomes invincible for " + _buffData.duration + " sec-";
        }
   }

    public void PurchaseBuffSkill()
    {
        shop.AddBuffSkill(_buffData.skillName);
    }

    public string GetBuffName()
    {
        return _buffData.skillName;
    }
}
