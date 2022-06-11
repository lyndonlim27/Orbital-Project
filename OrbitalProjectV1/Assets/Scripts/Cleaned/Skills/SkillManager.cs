using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    private void Start()
    {
        _promptText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSkillInput();
    }

    private void CheckSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2);
        }
    }

    private void UseSkill(int index)
    {
        SkillBehaviour skillBehaviour = this.transform.GetChild(index).GetComponent<SkillBehaviour>();
        if (skillBehaviour != null)
        {
            _promptText.text = skillBehaviour.PromptCheck();
            StartCoroutine(FlashPrompt());
            skillBehaviour.ActivateSkill();
        }
    }

    private IEnumerator FlashPrompt()
    {
        _promptText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _promptText.gameObject.SetActive(false);
    }

}
