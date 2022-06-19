using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchSystem : MonoBehaviour
{
    [SerializeField] public bool activated;
    [SerializeField] private int numseqs;
    [SerializeField] private int tries = 0;
    [SerializeField] private int levels;
    [SerializeField] private int numlights;
    [SerializeField] private int currindex;
    [SerializeField] private List<List<int>> sequence;
    [SerializeField] Light2D[] candles;
    public int numofCandles;
    public bool incoroutine { get; private set; }

    private void Awake()
    {
        currindex = 0;
        sequence = new List<List<int>>();
        activated = false;
        candles = GetComponentsInChildren<Light2D>();
        //Debug.Log("These are candles" + candles.Length);
        numofCandles = candles.Length;
    }
    
    private void StartCandleAnimation()
    {
        foreach(Light2D light2D in candles)
        {
            light2D.GetComponent<Animator>().SetBool("candlelights",true);
        }
    }

    public void Activate(int _numseqs)
    {
        Debug.Log("entered");
        numseqs = _numseqs;
        activated = true;
        numlights = candles.Length;
        currindex = 0;
        StartCandleAnimation();
        levels = Random.Range(3, 7);
        GeneratePattern();
        incoroutine = false;
    }

    private void GeneratePattern()
    {

        while (sequence.Count < numseqs)
        {
            List<int> currseq = new List<int>();
            for (int j = 0; j < levels; j++)
            {
                Debug.Log("Numlights = " + numlights);
                currseq.Add(Random.Range(0, numlights));
            }
            Debug.Log(currseq.Count);
            sequence.Add(currseq);
        }
        
    }

    public IEnumerator StartLightShow()
    {
        Debug.Log("entered");
        incoroutine = true;
        StartCandleAnimation();
        List<int> currentseq = sequence[currindex];
        foreach(int i in currentseq)
        {
            Debug.Log("current num: " + i);
            Debug.Log("we enterd coroutine");
            candles[i].intensity = 2f;
            Color c = candles[i].color;
            candles[i].color = Color.red;
            yield return new WaitForSeconds(1.5f);
            candles[i].intensity = 1f;
            candles[i].color = c;
            yield return new WaitForSeconds(1.5f);

        }
        incoroutine = false;

    }

    public List<int> GetCurrentSeq()
    {
        if(sequence != null)
        {
            return sequence[currindex];
        } else
        {
            return new List<int>();
        }
        
    }

    public void Next()
    {
        if (!IsComplete())
        {
            currindex++;
        }
        
    }

    public bool IsComplete()
    {
        return currindex == sequence.Count - 1;
    }

}
