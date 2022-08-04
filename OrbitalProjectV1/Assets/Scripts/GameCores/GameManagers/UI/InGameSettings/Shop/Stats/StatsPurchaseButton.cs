using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement.UIComps
{
    public class StatsPurchaseButton : MonoBehaviour
    {
        private enum STATS_TYPE
        {
            ADD_HEALTH,
            ADD_MANA,
            ADD_SPEED
        }
        private Button _button;
        private Shop _shop;
        [SerializeField] private STATS_TYPE _statsType;
        // Start is called before the first frame update
        void Start()
        {
            _shop = FindObjectOfType<Shop>(true);
            _button = GetComponent<Button>();
            switch (_statsType)
            {
                default:
                case STATS_TYPE.ADD_HEALTH:
                    _button.onClick.AddListener(_shop.AddMaxHealth);
                    break;
                case STATS_TYPE.ADD_MANA:
                    _button.onClick.AddListener(_shop.AddMaxMana);
                    break;
                case STATS_TYPE.ADD_SPEED:
                    _button.onClick.AddListener(_shop.AddMaxSpeed);
                    break;
            }

        }
    }
}
