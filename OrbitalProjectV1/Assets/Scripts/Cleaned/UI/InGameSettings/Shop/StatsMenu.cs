using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MenuBehaviour
{
    private Button _backButton;
    private Shop _shop;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _shop = GetComponentInParent<Shop>(true);
        _backButton = GetComponentsInChildren<Button>()[0];
        _backButton.onClick.AddListener(this.Inactive);
        _backButton.onClick.AddListener(_shop.GetComponentInChildren<ShopMainMenu>(true).Active);
    }


}
