using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSettings : MenuBehaviour
{
    private Button[] buttons;
    private ControlMenu _controlMenu;
    private Shop _shop;
    private WarningMenu _warningMenu;

    protected override void Start()
    {
        base.Start();
        _controlMenu = popUpSettings.GetComponentInChildren<ControlMenu>(true);
        _shop = popUpSettings.GetComponentInChildren<Shop>(true);
        _warningMenu = popUpSettings.GetComponentInChildren<WarningMenu>(true);
        buttons = GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(_controlMenu.Active);
        buttons[1].onClick.AddListener(_shop.Active);
        buttons[2].onClick.AddListener(_warningMenu.Active);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(Inactive);
        }
    }

}
