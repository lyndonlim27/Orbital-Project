using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeMenu : MenuBehaviour
{
    private DebuffPurchaseButton[] _debuffPurchaseButtons;
    private BuffPurchaseButton[] _buffPurchaseButtons;
    private Shop _shop;
    private Button _backButton;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _debuffPurchaseButtons = GetComponentsInChildren<DebuffPurchaseButton>(true);
        _buffPurchaseButtons = GetComponentsInChildren<BuffPurchaseButton>(true);
        _shop = GetComponentInParent<Shop>(true);
        foreach (DebuffPurchaseButton debuffButton in _debuffPurchaseButtons)
        {
            debuffButton.GetComponent<Button>().onClick.AddListener(debuffButton.PurchaseDebuffSkill);
        }
        foreach (BuffPurchaseButton buffButton in _buffPurchaseButtons)
        {
            buffButton.GetComponent<Button>().onClick.AddListener(buffButton.PurchaseBuffSkill);
        }
        _backButton = GetComponentsInChildren<Button>()[0];
        _backButton.onClick.AddListener(this.Inactive);
        _backButton.onClick.AddListener(_shop.GetComponentInChildren<ShopMainMenu>(true).Active);

    }
}
