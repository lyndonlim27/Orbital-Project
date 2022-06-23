using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private Image _promptImage;
    private TextMeshProUGUI _promptText;
    public bool loggedIn { get; private set;}
    public bool loaded;
    public string currScene { get; private set; }

    [Header("for testing")]
    [SerializeField] private string _email;
    [SerializeField] private string _password;


    private GameData _gameData;
    private List<IDataPersistence> _dataPersistences;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Please put only one data manager");
        }
        instance = this;
    }

    private void Start()
    {
        loggedIn = false;
        loaded = false;
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _promptImage = GameObject.Find("Prompt").GetComponent<Image>();
            _promptText = _promptImage.GetComponentInChildren<TextMeshProUGUI>(true);
        }
        //LoadGame();
    }



    public void NewGame()
    {
        this._gameData = new GameData();
    }

    [ContextMenu("Load")]
    public void LoadGame()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataLoad, OnError);
    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        this._gameData = new GameData();
        _dataPersistences = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.SaveData(ref _gameData);
        }
        string dataToStore = JsonUtility.ToJson(_gameData, true);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Player", dataToStore }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = GameObject.Find("Email").GetComponent<TMP_InputField>().text,
            TitleId = "B1035"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetSuccess, OnErrorReset);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = GameObject.Find("Email").GetComponent<TMP_InputField>().text,
            Password = GameObject.Find("Password").GetComponent<TMP_InputField>().text,
            RequireBothUsernameAndEmail = false 
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnErrorRegister);
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            //Email = "hnghng127@gmail.com",
            //Password = "123456"
            Email = GameObject.Find("Email").GetComponent<TMP_InputField>().text,
            Password = GameObject.Find("Password").GetComponent<TMP_InputField>().text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnErrorLogin);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SaveGame();
        //if (SceneManager.GetActiveScene().name == "MainMenu")
        //{
        //    FindObjectOfType<LoginMenu>(true).gameObject.SetActive(false);
        _promptText.text = "Registered successfully. Please login now";
        StartCoroutine(FlashPrompt());
        //    LoadGame();
        //}
    }

    private void OnLoginSuccess(LoginResult result)
    {
        loggedIn = true;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindObjectOfType<LoginMenu>(true).gameObject.SetActive(false);
            _promptText.text = "Login successfully";
            StartCoroutine(FlashPrompt());
            LoadGame();
        }
            Debug.Log("Logged in");
    }

    private void OnResetSuccess(SendAccountRecoveryEmailResult result)
    {
        _promptText.text = "Password reset email sent!";
        StartCoroutine(FlashPrompt());
    }

    private void OnErrorReset(PlayFabError error)
    {
        _promptText.text = "User does not exist";
        StartCoroutine(FlashPrompt());
    }

    private void OnErrorLogin(PlayFabError error)
    {

        _promptText.text = "Incorrect Email/Password";
        StartCoroutine(FlashPrompt());
    }

    private void OnErrorRegister(PlayFabError error)
    {
        //_promptText.text = "Email already exists or invalid/ Password must be 6 characters long";
        Debug.Log(error.ToString());

        if (error.ToString().Contains("exists"))
        {
            _promptText.text = "Email already exists" + "\n";
        }
        if (error.ToString().Contains("Email address is not valid"))
        {
            _promptText.text += "Email address is not valid" + "\n";
        }
        if(error.ToString().Contains("6 and 100"))
        {
            _promptText.text += "Password must be at least 6 characters long";
        }
        StartCoroutine(FlashPrompt());
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Erorr");
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Saved");
    }

    private void OnDataLoad(GetUserDataResult result)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            GameData loadedData = JsonUtility.FromJson<GameData>(result.Data["Player"].Value);
            currScene = loadedData.currScene;
        }
        else
        {
            loaded = false;
            StartCoroutine(LoadCoroutine(result));
        }
    }


    private IEnumerator LoadCoroutine(GetUserDataResult result)
    {
        GameData loadedData = JsonUtility.FromJson<GameData>(result.Data["Player"].Value);
        currScene = loadedData.currScene;
        _dataPersistences = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.LoadData(loadedData);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        loaded = true;

    }

    private IEnumerator FlashPrompt()
    {
        _promptImage.enabled = true;
        _promptText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _promptImage.enabled = false;
        _promptText.gameObject.SetActive(false);
        _promptText.text = "";
    }

    [ContextMenu("Register")]
    public void TestingRegister()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = this._email,
            Password = this._password,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    [ContextMenu("Login")]
    public void TestingLogin()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = this._email,
            Password = this._password,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

}




