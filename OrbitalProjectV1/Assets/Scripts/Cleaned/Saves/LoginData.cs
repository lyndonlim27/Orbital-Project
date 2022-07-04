using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LoginData
{

    public string userName;
    public int userPin;
    public int userScore;
    public PlayerData playerData;

    public LoginData(string username, int pin, int score)
    {
        this.userName = username;
        this.userScore = score;
        this.userPin = pin;
        //this.playerData = playerData;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
