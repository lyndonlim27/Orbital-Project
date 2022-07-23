using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConsumableItemData : EntityData
{
    public int _gold;
    public int _health;
    public int _mana;
    public enum CONSUMABLE
    {
        HEALTH,
        GOLD,
        LETTER,
        MANA,
        FRAGMENTS,
    }
    public CONSUMABLE _consumableType;
    public Sprite[] letters;
    public char letter;


    private void Awake()
    {
        letters = Resources.LoadAll<Sprite>("Sprites/keyboardletters");
    }

}
