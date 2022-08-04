using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EntityCores.PlayerCore;
using GameManagement;

namespace PuzzleCores
{
    public class QuizPuzzle : MonoBehaviour, Puzzle
    {
        private PuzzleInputManager puzzleInputManager;
        private Dictionary<string, int> questions;
        [SerializeField] private int curr;
        private string currquestion;
        private string operators;
        private int lastseq;
        public bool activated;


        private void Awake()
        {
            operators = "+-x";
            questions = new Dictionary<string, int>();
            puzzleInputManager = FindObjectOfType<PuzzleInputManager>(true);
            activated = false;

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (puzzleInputManager.guessed)
            {
                if (CheckInput() && !IsComplete())
                {
                    Next();
                }
                else
                {
                    PullUpPuzzleMenu();
                }

            }
        }

        public bool IsComplete()
        {
            if (activated)
            {
                return curr >= lastseq;
            }
            else
            {
                return false;
            }

        }

        public void ActivatePuzzle(int _seq)
        {
            lastseq = _seq;
            activated = true;
            for (int i = 0; i < _seq; i++)
            {

                int rand = Random.Range(0, operators.Length);
                char curroperator = operators[rand];
                int randomnum1 = Random.Range(0, 999);
                int randomnum2 = Random.Range(0, 999);
                if (randomnum1 < randomnum2 && curroperator == '-')
                {
                    int temp = randomnum1;
                    randomnum1 = randomnum2;
                    randomnum2 = temp;
                }
                string question = $"{randomnum1} {curroperator} {randomnum2} = ?";
                questions[question] = GetAnswer(randomnum1, randomnum2, curroperator);

                if (i == 0)
                {
                    currquestion = question;
                }

            }
            PullUpPuzzleMenu();


        }

        private int GetAnswer(int randomnum1, int randomnum2, char curroperator)
        {
            int ans;
            switch (curroperator)
            {
                default:
                    ans = -1;
                    break;
                case '+':
                    ans = randomnum1 + randomnum2;
                    break;
                case '-':

                    ans = randomnum1 - randomnum2;
                    break;
                case 'x':
                    ans = randomnum1 * randomnum2;
                    break;
            }

            return ans;
        }

        private void PullUpPuzzleMenu()
        {
            puzzleInputManager.SetUpText(currquestion);
            puzzleInputManager.SetUp(questions[currquestion].ToString().Length);
            puzzleInputManager.gameObject.SetActive(true);
            FindObjectOfType<SkillManager>().enabled = false;

        }

        public bool CheckInput()
        {
            int ans = questions[currquestion];
            string guess = "";
            foreach (LetterSlotNoDnD slot in puzzleInputManager.GetCurrentGuess())
            {
                if (slot.currnum != -1)
                {
                    guess += slot.currnum;
                }

            }

            puzzleInputManager.ResetGuess();
            if (guess != "")
            {
                return int.Parse(guess) == ans;
            }
            else
            {
                return false;
            }



        }

        public void Next()
        {
            curr++;
            if (curr < lastseq)
            {
                currquestion = questions.Keys.ToList()[curr];
                PullUpPuzzleMenu();
            }

        }

        public void Fulfill()
        {
            throw new System.NotImplementedException();
        }

        public bool IsActivated()
        {
            return activated;
        }
    }
}
