using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackPurchaseButton : SkillPurchaseButton
{
    [SerializeField] private AttackData _attackData;
    private string _skillTitle;
    private List<AttackData> _shurikenDatas;
    private List<AttackData> _dashDatas;
    // Start is called before the first frame update

    protected override void OnEnable()
    {
        base.OnEnable();
        if (player.GetAttackData() != null)
        {
            shop.SetAttackButtons(player.GetAttackData().skillName);
        }
    }

    protected override void Start()
    {
        base.Start();
        _shurikenDatas = new List<AttackData>();
        _dashDatas = new List<AttackData>();
        for (int i = 1; i < 4; i++)
        {
            _shurikenDatas.Add(Resources.Load<AttackData>("Data/SkillData/Shuriken" + i + "Data"));
        }
        for (int i = 1; i < 4; i++)
        {
            _dashDatas.Add(Resources.Load<AttackData>("Data/SkillData/Dash" + i + "Data"));
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
            if (_attackData.skillName.Contains("Fireball"))
            {
                int level = int.Parse(_attackData.skillName.Substring(8, 1));
                _skillTitle = "Shuriken (Level " + level + ")";
                _attackData = _shurikenDatas[level - 1];
            }
            else
            {
                int level = int.Parse(_attackData.skillName.Substring(9, 1));
                _skillTitle = "Dash (Level " + level + ")";
                _attackData = _dashDatas[level - 1];
            }
        }
        else
        {
            if (_attackData.skillName.Contains("Fireball"))
            {
                _skillTitle = "Fireball (Level " + _attackData.skillName.Substring(8, 1) + ")";
            }
            else
            {
                _skillTitle = "Shockwave (Level " + _attackData.skillName.Substring(9, 1) + ")";
            }
        }

        image.overrideSprite = _attackData.sprite;
        textDisplay.text = _skillTitle + "\n" +
            "Damage: - " + _attackData.damage + " HP" + "\n";

        if (_skillTitle.Contains("Fireball"))
        {
            textDisplay.text += "Fireballs: " + _attackData.numOfProjectiles + "\n";
        }
        else if (_skillTitle.Contains("Shuriken"))
        {
            textDisplay.text += "Shurikens: " + _attackData.numOfProjectiles + "\n";
        }
        else if (_skillTitle.Contains("Shockwave"))
        {
            textDisplay.text += "Shockwaves: " + _attackData.numOfProjectiles + "\n";
        }

        textDisplay.text += "Cooldown: " + _attackData.cooldown + " sec" + "\n" +
                "Mana Cost: " + _attackData.manaCost + " MP" + "\n" +
                "Cost: " + _attackData.goldCost + " gold" + "\n";

        if (_skillTitle.Contains("Fireball"))
        {
            textDisplay.text += "-Shoots out " + _attackData.numOfProjectiles + " fireballs-";
        }
        else if (_skillTitle.Contains("Shuriken"))
        {
            textDisplay.text += "-Spins " + _attackData.numOfProjectiles + " daggers around-";
        }
        else if (_skillTitle.Contains("Dash"))
        {
            textDisplay.text += "-Dash and deal damage in direction faced and slash at the end";
        }
        else
        {
            textDisplay.text += "Fires out " + _attackData.numOfProjectiles + " shockwaves that does damage";
        }
    }

    public void PurchaseAttackSkill()
    {
        shop.AddAttackSkill(_attackData.skillName);
    }

    public string GetAttackName()
    {
        return _attackData.skillName;
    }
}
