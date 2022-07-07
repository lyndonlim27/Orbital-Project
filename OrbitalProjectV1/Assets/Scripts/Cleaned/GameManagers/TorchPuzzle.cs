using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TorchPuzzle : MonoBehaviour, Puzzle
{
    List<ItemWithTextBehaviour> torchLights;
    HealingBehaviour healingFountain;
    [SerializeField] private string unlockcode;
    [SerializeField] private string input;
    [SerializeField] private int numtorches;
    UITextDescription uITextDescription;

    public bool activated;
    public bool isComplete;

    private void Awake()
    {
        torchLights = new List<ItemWithTextBehaviour>();
        healingFountain = GetComponentInChildren<HealingBehaviour>(true);
        activated = false;
        isComplete = false;
        uITextDescription = FindObjectOfType<UITextDescription>(true);
    }

    private void Update()
    {
        numtorches = torchLights.Count;
    }

    public void ActivatePuzzle(int seq)
    {
        GetRandomOrder();
        activated = true;
    }

    public void AddPuzzleTorch(ItemWithTextBehaviour torch)
    {
        torchLights.Add(torch);
    }

    private void GetRandomOrder()
    {
        for (int i = 0; i < torchLights.Count; i++)
        {
            int rand = Random.Range(0, torchLights.Count);
            unlockcode += torchLights[rand].GetInstanceID();
        }
    }

    public void RandomLightUp()
    {
        int rand = Random.Range(0, torchLights.Count - 1);
        List<int> lightthese = new List<int>();
        for (int i = 0; i < rand; i++)
        {
            int r = Random.Range(0, torchLights.Count);
            lightthese.Add(r);

        }

        for (int i = 0; i < torchLights.Count; i++)
        {
            if (lightthese.Contains(i))
            {
                torchLights[i].LightUp();
            }
            else
            {
                torchLights[i].ResetLight();
            }
        }
    }

    public void Input(ItemWithTextBehaviour torchlight)
    {
        input += torchlight.GetInstanceID();
        CheckInput();
    }

    private void CheckInput()
    {
        if (input.Contains(unlockcode))
        {
            LightUpAllTorch();
        }
        else
        {
            RandomLightUp();

        }
    }

    private void LightUpAllTorch()
    {
        foreach (ItemWithTextBehaviour torch in torchLights)
        {
            if (!torch.lit)
                torch.LightUp();
            torch.enabled = false;
        }

        StartCoroutine(LightUpStatue());
    }


    private IEnumerator LightUpStatue()
    {
        healingFountain.enabled = true;
        if (uITextDescription.isActiveAndEnabled)
        {
            uITextDescription.StartDescription("The statue lights up...");
        }
        yield return new WaitForSeconds(2f);
        isComplete = true;

    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }

    public void Fulfill()
    {
        throw new System.NotImplementedException();
    }

    public bool IsComplete()
    {
        throw new System.NotImplementedException();
    }

    public bool IsActivated()
    {
        return activated;
    }

    //public void RandomizePuzzlePlacement()
    //{
    //    foreach()
    //}
}