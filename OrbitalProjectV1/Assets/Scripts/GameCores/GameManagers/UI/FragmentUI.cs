using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FragmentUI : MonoBehaviour
{

    public static FragmentUI instance { get; private set; }
    private TextMeshProUGUI _text;
    private Image _image;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFragmentUI(int fragments)
    {
        _text.text = fragments + "/5";
        _image.fillAmount = fragments / 5f;
    }

    public bool IsComplete()
    {
        return _image.fillAmount == 1;
    }
}
