using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TorchPuzzle : MonoBehaviour, Puzzle
{
    List<ItemWithTextData> puzzleTorchesData;
    [SerializeField] List<ItemWithTextBehaviour> torchLights;
    RoomManager currRoom;
    HealingBehaviour healingFountain;
    [SerializeField] private string unlockcode;
    [SerializeField] private string remainingcode;
    [SerializeField] private string input;
    [SerializeField] private int numtorches;
    UITextDescription uITextDescription;
    int currindex;

    public bool activated;
    public bool isComplete;

    private void Awake()
    {
        torchLights = new List<ItemWithTextBehaviour>();
        puzzleTorchesData = new List<ItemWithTextData>();
        numtorches = 4;
        CreateTorchdata();
        healingFountain = GetComponentInChildren<HealingBehaviour>(true);
        activated = false;
        isComplete = false;
        uITextDescription = FindObjectOfType<UITextDescription>(true);
        currindex = 0;
    }

    private void CreateTorchdata()
    {
        var puzzleTorchdata = Resources.Load("Data/ItemWithText/PuzzleTorch1") as ItemWithTextData;
        for (int i = 0; i < numtorches; i++)
        {
            var datacopy = Instantiate(puzzleTorchdata);
            puzzleTorchesData.Add(datacopy);

        }
    }

    private void Update()
    {
    }

    public void ActivatePuzzle(int seq)
    {
        activated = true;
        GetRandomOrder();
        remainingcode = unlockcode;

    }

    public void SpawnTorches()
    {
        currRoom.SpawnObjects(puzzleTorchesData.ToArray());
        GetTorches();
        RandomizeTorches();
    }

    public void GetTorches()
    {
        torchLights = GetComponentsInChildren<ItemWithTextBehaviour>().ToList();
    }

    //public void AddPuzzleTorch(ItemWithTextBehaviour torch)
    //{
    //    torchLights.Add(torch);
    //    torch.SetTorchPuzzle(this);
    //}

    private void RandomizeTorches()
    {
        torchLights[0].transform.localPosition = new Vector3(-10f,10f);
        torchLights[1].transform.localPosition = new Vector3(10f, 10f);
        torchLights[2].transform.localPosition = new Vector3(-10f, -10f);
        torchLights[3].transform.localPosition = new Vector3(10f, -10f);


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
            string curr = torchLights[i].GetInstanceID().ToString();
            if (lightthese.Contains(i) || remainingcode.StartsWith(curr))
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
        input = torchlight.GetInstanceID().ToString();
        CheckInput(input);
    }

    private void CheckInput(string input)
    {
        if (remainingcode.StartsWith(input))
        {
            remainingcode = ReplaceFirstOccurrence(remainingcode, input, "");
            RandomLightUp();
            uITextDescription.StartDescription("----");
        }
        else
        {
            remainingcode = unlockcode;
            uITextDescription.StartDescription("Wrong code, please reattempt");
        }

        if (remainingcode == "")
        {
            LightUpAllTorch();
        }
           
    }


    /**
     * Replace the first occurence of the string.
     * @return the new string with the first char replaced.
     */
    private string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.IndexOf(Find);

        string result = Source.Remove(Place, Find.Length).Insert(Place, Replace.ToString());
        return result;
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
        return isComplete;
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void SetCurrentRoom(RoomManager room)
    {
        this.currRoom = room;
    }

    //public void RandomizePuzzlePlacement()
    //{
    //    foreach()
    //}

    // 1 4 3 2 -> hit torch 4. -> random num of torches -> one of the torch is 1. ->  
}