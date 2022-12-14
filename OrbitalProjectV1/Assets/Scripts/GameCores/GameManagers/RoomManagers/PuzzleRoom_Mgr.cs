using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleCores;

namespace GameManagement.RoomManagers
{
    public class PuzzleRoom_Mgr : RoomManager
    {
        public enum PUZZLE_TYPE
        {
            QUIZ,
            SOCCER,
            LASER,
            LIGHTSWITCH,
            TORCH,
            PRESSURE,
            COUNT,
        }

        public PUZZLE_TYPE puzzle_type;
        private Puzzle puzzle;
        private MonoBehaviour puzzleMono;
        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
            //puzzle_type = (PUZZLE_TYPE)Random.Range(0, (int) PUZZLE_TYPE.COUNT);
            puzzle_type = PUZZLE_TYPE.PRESSURE;
        }
        protected override void Start()
        {
            base.Start();
            StartCoroutine(WaitForTerrainLoad());
        }

        private IEnumerator WaitForTerrainLoad()
        {
            while (pointsInRoomBound == null)
            {
                yield return null;
            }
            LoadPuzzle();
        }

        private void LoadPuzzle()
        {
            switch (puzzle_type)
            {
                default:
                case PUZZLE_TYPE.QUIZ:
                    var quiz = gameObject.AddComponent<QuizPuzzle>();
                    puzzle = quiz;
                    puzzleMono = quiz;
                    break;
                case PUZZLE_TYPE.SOCCER:
                    var soccer = gameObject.AddComponent<SoccerPuzzle>();
                    puzzle = soccer;
                    puzzleMono = soccer;
                    break;
                case PUZZLE_TYPE.LASER:
                    var laser = gameObject.AddComponent<LaserPuzzle>();
                    puzzle = laser;
                    puzzleMono = laser;
                    break;
                case PUZZLE_TYPE.LIGHTSWITCH:
                    var lightswitch = gameObject.AddComponent<LightUpPuzzle>();
                    puzzle = lightswitch;
                    puzzleMono = lightswitch;
                    break;
                case PUZZLE_TYPE.TORCH:
                    GameObject torchprefab = Resources.Load("PuzzlePrefab/TorchLightPuzzle") as GameObject;
                    GameObject go = Instantiate(torchprefab);
                    go.name = "TorchLightPuzzle";
                    go.transform.SetParent(this.transform);
                    GetAReasonablePoint(go);
                    var torchPuzzle = go.GetComponent<TorchPuzzle>();
                    torchPuzzle.SetCurrentRoom(this);
                    torchPuzzle.SpawnTorches();
                    puzzle = torchPuzzle;
                    puzzleMono = torchPuzzle;
                    break;
                case PUZZLE_TYPE.PRESSURE:
                    var pressurePuzzle = gameObject.AddComponent<PressurePuzzle1>();
                    pressurePuzzle.CreatePushableData();
                    pressurePuzzle.CreateSwitchDatas();
                    puzzle = pressurePuzzle;
                    puzzleMono = pressurePuzzle;
                    break;
            }
            puzzleMono.enabled = false;
        }

        private void GetAReasonablePoint(GameObject go)
        {
            var copy = new List<Vector3Int>(pointsInRoomBound);
            var hold = new List<Vector3Int>();
            var startingRad = 5f;
            int iterations = 100;
            Vector3Int currpoint;
            do
            {
                currpoint = copy[Random.Range(0, copy.Count)];
                if (iterations == 1 && startingRad > 0)
                {
                    startingRad--;
                    iterations = 100;
                    copy = hold;
                }
                copy.Remove(currpoint);
                hold.Add(currpoint);
            } while (Physics2D.OverlapCircle((Vector2Int)currpoint, startingRad, LayerMask.GetMask("Obstacles", "Doors", "PassableDeco", "Mirror", "HouseExterior", "HouseInterior")) && iterations-- > 0);
            go.transform.position = currpoint;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (activated && puzzle != null && !puzzle.IsActivated())
            {
                puzzleMono.enabled = true;
                puzzle.ActivatePuzzle(Random.Range(3, 7));
            }
            RoomChecker();
            CheckRunningEvents();

        }

        protected override bool CanProceed()
        {
            return puzzle != null && activated && puzzle.IsComplete();
        }

        //protected override void RoomChecker()
        //{
        //    if (puzzle == null)
        //    {
        //        return;
        //    }
        //    base.RoomChecker();
        //}
    }
}