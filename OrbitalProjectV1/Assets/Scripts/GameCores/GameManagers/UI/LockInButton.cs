using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement.UIComps
{
    public class LockInButton : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            PuzzleInputManager mono = GetComponentInParent<PuzzleInputManager>();
            GetComponent<Button>().onClick.AddListener(() => mono.OnButtonClicked());
        }
    }
}
