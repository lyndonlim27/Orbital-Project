using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMenu : MenuBehaviour
{
    private Button[] _buttons;
    private LoadScene _loadMainScene;
    private MainSettings _mainSettings;

    protected override void Start()
    {
        base.Start();
        _buttons = GetComponentsInChildren<Button>(true);
        _loadMainScene = FindObjectOfType<LoadScene>(true);
        _mainSettings = popUpSettings.GetComponentInChildren<MainSettings>(true);
        _buttons[0].onClick.AddListener(_loadMainScene.LoadMainMenu);
        _buttons[1].onClick.AddListener(this.Inactive);
        _buttons[1].onClick.AddListener(_mainSettings.Active);
    }
}
