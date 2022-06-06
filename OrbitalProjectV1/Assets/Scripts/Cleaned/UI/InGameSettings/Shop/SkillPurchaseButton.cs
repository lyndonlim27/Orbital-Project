using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPurchaseButton : MonoBehaviour
{
    [SerializeField] protected string skillName;
    protected Shop shop;

    private void Start()
    {
        shop = FindObjectOfType<Shop>(true);
    }
    // Start is called before the first frame update

    public void PurchaseDebuffSkill()
    {
        shop.AddDebuffSkill(skillName);
    }

    public void PurchaseBuffSkill()
    {
        shop.AddBuffSkill(skillName);
    }
}
