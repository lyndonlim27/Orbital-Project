using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    private void Start()
    {
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
            skillBehaviour.ActivateSkill();
        }
    }

}
