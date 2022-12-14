using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement.UIComps
{
    public class ShopMainMenu : MenuBehaviour
    {
        private Button[] buttons;
        private Shop _shop;
        private SkillTreeMenu _skilTreeMenu;
        private MainSettings _mainSettings;
        private StatsMenu _statsMenu;

        protected override void Start()
        {
            base.Start();
            buttons = GetComponentsInChildren<Button>();
            _shop = GetComponentInParent<Shop>();
            _skilTreeMenu = _shop.GetComponentInChildren<SkillTreeMenu>(true);
            _statsMenu = _shop.GetComponentInChildren<StatsMenu>(true);
            _mainSettings = popUpSettings.GetComponentInChildren<MainSettings>(true);
            buttons[0].onClick.AddListener(_mainSettings.Active);
            buttons[0].onClick.AddListener(_shop.Inactive);
            buttons[1].onClick.AddListener(_skilTreeMenu.Active);
            buttons[1].onClick.AddListener(Inactive);
            buttons[2].onClick.AddListener(_statsMenu.Active);
            buttons[2].onClick.AddListener(Inactive);
        }
    }
}
