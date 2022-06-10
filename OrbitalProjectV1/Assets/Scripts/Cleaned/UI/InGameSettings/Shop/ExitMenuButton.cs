using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenuButton : MonoBehaviour
{
    private PopUpSettings _popUpSettings;
    // Start is called before the first frame update
    void Start()
    {
        _popUpSettings = GetComponentInParent<PopUpSettings>();
        GetComponent<Button>().onClick.AddListener(_popUpSettings.Inactive);;
    }
}
