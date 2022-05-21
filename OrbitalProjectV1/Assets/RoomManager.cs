using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomManager : MonoBehaviour
{
    protected List<string> conditions;

    public void FulfillCondition(string key)
    {
        this.conditions.Remove(key);
    }


    protected bool CanProceed()
    {
        
        return conditions.Count == 0;
    }

    protected void CheckDialogue()
    {
        if (GameObject.FindObjectOfType<DialogueManager>().playing)
        {
            //// for now we just use enemy tags, i think next time if got other stuffs then see how
            PauseGame();
        }
        else
        {
            ResumeGame();

        }
    }

    private static void ResumeGame()
    {
        foreach (Entity entity in GameObject.FindObjectsOfType<Entity>())
        {
            entity.enabled = true;
            entity.animator.enabled = true;
        }
    }

    private static void PauseGame()
    {
        foreach (Entity entity in GameObject.FindObjectsOfType<Entity>())
        {
            entity.enabled = false;
            entity.animator.enabled = false;

        }
    }
}
