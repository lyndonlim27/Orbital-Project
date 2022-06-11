using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenu : MenuBehaviour
{
    private MainSettings _mainSettings;
    // Start is called before the first frame update
    protected override void Start()
    {
        _mainSettings = this.transform.parent.GetComponentInChildren<MainSettings>(true);
        Button button = GetComponentInChildren<Button>(true);
        button.onClick.AddListener(Inactive);
        button.onClick.AddListener(_mainSettings.Active);
    }
    
}
