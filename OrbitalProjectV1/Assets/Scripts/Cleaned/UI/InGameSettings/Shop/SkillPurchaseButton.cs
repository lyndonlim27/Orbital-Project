using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillPurchaseButton : MonoBehaviour
{
    [SerializeField] protected string skillName;
    protected Shop shop;

    private void Start()
    {
        shop = FindObjectOfType<Shop>(true);
    }
    // Start is called before the first frame update

    public abstract void PurchaseSkill();
}
