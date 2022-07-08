using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public enum STATS_TYPE
    {
        MAX_HEALTH,
        MAX_MANA,
        MAX_SPEED
    }
    public STATS_TYPE _statsType;
    private Player _player;
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _text = GetComponent<TextMeshProUGUI>();
    }


    public void UpdateStatsText()
    {
        switch (_statsType)
        {
            default:
            case STATS_TYPE.MAX_HEALTH:
                _text.text = _player.GetMaxHealth().ToString();
                break;
            case STATS_TYPE.MAX_MANA:
                _text.text = _player.GetMaxMana().ToString();
                break;
            case STATS_TYPE.MAX_SPEED:
                _text.text = _player.GetSpeed().ToString();
                break;

        }
    }
}
