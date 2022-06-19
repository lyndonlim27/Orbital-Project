using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockInButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PuzzleInputManager mono = GetComponentInParent<PuzzleInputManager>();
        GetComponent<Button>().onClick.AddListener(() => mono.OnButtonClicked());
    }
}
