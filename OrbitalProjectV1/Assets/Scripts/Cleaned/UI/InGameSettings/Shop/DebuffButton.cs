using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffButton : SkillPurchaseButton
{

    public override void PurchaseSkill()
    {
        shop.AddDebuffSkill(skillName);
    }
}
