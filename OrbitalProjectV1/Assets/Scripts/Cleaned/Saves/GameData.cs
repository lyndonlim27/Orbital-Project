using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //[SerializeField] private string id;
    //[ContextMenu("Generate id")]
    //private void GenerateGuid()
    //{
    //    id = System.Guid.NewGuid().ToString();
    //}

    
    /*
     * Player Data
     */
    public int currHealth;
    public int maxHealth;
    public int currMana;
    public int maxMana;
    public int currGold;
    public string currWeapon;
    public Vector2 currPos;
    public string debuffDataName;
    public string buffDataName;
    public string attackDataName;
    public float moveSpeed;
    public string currScene;
    public bool ranged;

    /*
     * Room Data
     */
    public SerializableDictionary<string, int> rooms;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.currHealth = 100;
        this.maxHealth = 100;
        this.currMana = 100;
        this.maxMana = 100;
        this.currGold = 0;
        this.currWeapon = "Fists";
        this.currPos = Vector2.zero;
        this.attackDataName = null;
        this.buffDataName = null;
        this.debuffDataName = null;
        moveSpeed = 5;
        currScene = "ProjectConceptScene3";
        this.ranged = true;
        this.rooms = new SerializableDictionary<string, int>();
    }
}
