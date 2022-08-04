using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using GameManagement;

namespace PuzzleCores
{
    public class LightSwitchSystem : MonoBehaviour, Puzzle
    {
        [SerializeField] public bool activated;
        [SerializeField] private int numseqs;
        //[SerializeField] private int tries = 0;
        [SerializeField] private int levels;
        [SerializeField] private int numlights;
        [SerializeField] private int currindex;
        [SerializeField] private List<List<int>> sequence;
        [SerializeField] Light2D[] candles;
        [SerializeField] PuzzleBox puzzleBox;
        private UITextDescription uITextDescription;
        public int numofCandles;
        private RoomManager currroom;
        public bool incoroutine { get; private set; }

        private void Awake()
        {
            currindex = 0;
            sequence = new List<List<int>>();
            activated = false;
            candles = GetComponentsInChildren<Light2D>();
            uITextDescription = FindObjectOfType<UITextDescription>(true);
            //Debug.Log("These are candles" + candles.Length);
            numofCandles = candles.Length;
        }

        private void StartCandleAnimation()
        {
            foreach (Light2D light2D in candles)
            {
                light2D.GetComponent<Animator>().SetBool("candlelights", true);
            }
        }

        public void RandomizeCandlesPos()
        {

            candles[0].transform.position = currroom.transform.position + new Vector3(-10, 5);
            candles[1].transform.position = currroom.transform.position + new Vector3(-5, 5);
            candles[2].transform.position = currroom.transform.position + new Vector3(5, 5);
            candles[3].transform.position = currroom.transform.position + new Vector3(10, 5);

        }

        public void ActivatePuzzle(int _numseqs)
        {
            numseqs = _numseqs;
            activated = true;
            numlights = candles.Length;
            currindex = 0;
            StartCandleAnimation();
            levels = 5;
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
                    currseq.Add(Random.Range(0, numlights));
                }
                sequence.Add(currseq);
            }

        }

        public IEnumerator StartLightShow()
        {
            incoroutine = true;
            StartCandleAnimation();
            List<int> currentseq = sequence[currindex];
            foreach (int i in currentseq)
            {
                candles[i].intensity = 2f;
                Color c = candles[i].GetComponent<SpriteRenderer>().color;
                //candles[i].color = Color.red;
                candles[i].GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(1.5f);
                candles[i].intensity = 1f;
                candles[i].GetComponent<SpriteRenderer>().color = c;
                yield return new WaitForSeconds(1.5f);

            }
            incoroutine = false;

        }

        public List<int> GetCurrentSeq()
        {
            if (sequence != null)
            {
                return sequence[currindex];
            }
            else
            {
                return new List<int>();
            }

        }

        public void Next()
        {
            currindex++;
            if (!IsComplete())
            {
                uITextDescription.StartDescription("Light reflickers..");
                StartCoroutine(StartLightShow());
            }

        }

        public bool IsComplete()
        {
            return activated && currindex == sequence.Count;
        }

        protected void ActivatePuzzle()
        {
            throw new System.NotImplementedException();
        }

        public void Fulfill()
        {
            throw new System.NotImplementedException();
        }

        public bool IsActivated()
        {
            return activated;
        }

        public void SetCurrentRoom(RoomManager room)
        {
            currroom = room;
        }
    }
}
