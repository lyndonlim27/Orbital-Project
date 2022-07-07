using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom_Mgr : RoomManager
{
    public enum PUZZLE_TYPE
    {
        QUIZ,
        SOCCER,
        LASER,
        COUNT,
    }

    public PUZZLE_TYPE puzzle_type;
    private Puzzle puzzle;
    private MonoBehaviour puzzleMono;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        puzzle_type = (PUZZLE_TYPE)Random.Range(0, (int) PUZZLE_TYPE.COUNT);
    }
    void Start()
    {
        LoadPuzzle();
    }

    private void LoadPuzzle()
    {
        switch(puzzle_type)
        {
            default:
            case PUZZLE_TYPE.QUIZ:
                var quiz  = gameObject.AddComponent<QuizPuzzle>();
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
        }
        puzzleMono.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (activated && !puzzle.IsActivated())
        {
            puzzleMono.enabled = true;
            puzzle.ActivatePuzzle(Random.Range(3,7));
        } 
        RoomChecker();
        CheckRunningEvents();

    }

    protected override bool CanProceed()
    {
        return puzzle.IsComplete();
    }
}
