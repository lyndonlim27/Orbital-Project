using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Proyecto26;

public class LoadSave : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    [SerializeField] private TMP_InputField nameText;
    [SerializeField] private TMP_InputField pinText;
    [SerializeField] private PlayerData playerData;
    private System.Random random = new System.Random();

    private int _playerScore;
    private string _inputName;
    private int _inputPin;


    private LoginData _loginData;
    // Start is called before the first frame update
    void Start()
    {
        _playerScore = random.Next(1, 10);
        scoreText.text = "Score: " + _playerScore;
    }

    public void OnSubmit()
    {
        GetInput();
        PostToDatabase();
    }

    private void GetInput()
    {
        _inputName = nameText.text;
        _inputPin = int.Parse(pinText.text);
    }

    public void OnRetrieve()
    {
        GetInput();
        RetrieveFromDatabase();
    }

    private void PostToDatabase()
    {
        LoginData playerSave = new LoginData(_inputName, _inputPin, _playerScore);
        RestClient.Put("https://orbital-19ab0-default-rtdb.asia-southeast1.firebasedatabase.app/" + _inputName + ".json", playerSave);
        RestClient.Put("https://orbital-19ab0-default-rtdb.asia-southeast1.firebasedatabase.app/" + _inputName + ".json", playerData);
    }

    private void RetrieveFromDatabase()
    {
        RestClient.Get<LoginData>("https://orbital-19ab0-default-rtdb.asia-southeast1.firebasedatabase.app/" + _inputName + ".json").Then(response =>
        {
            _loginData = response;
            if(_loginData.userPin == _inputPin)
            {
                LoadData();

            }
            else
            {
            }
        });
    }
    
    private void LoadData()
    {
        scoreText.text = _loginData.userScore.ToString();
    }


}
