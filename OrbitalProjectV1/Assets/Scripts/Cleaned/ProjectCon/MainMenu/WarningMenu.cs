using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMenu : MenuBehaviour
{
    private Button[] _buttons;
    private LoadMainScene _loadMainScene;
    private MainSettings _mainSettings;

    protected override void Start()
    {
        base.Start();
        _buttons = GetComponentsInChildren<Button>();
        _loadMainScene = FindObjectOfType<LoadMainScene>(true);
        _mainSettings = popUpSettings.GetComponentInChildren<MainSettings>(true);
        _buttons[0].onClick.AddListener(_loadMainScene.LoadScene);
        _buttons[1].onClick.AddListener(this.Inactive);
        _buttons[1].onClick.AddListener(_mainSettings.Active);
    }
}
