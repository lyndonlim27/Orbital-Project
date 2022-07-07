using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoomMgr : RoomManager
{
    public enum PUZZLE_TYPE
    {
        QUIZ,
        SOCCER,
    }

    public PUZZLE_TYPE puzzle_type;
    private Puzzle puzzle;
    // Start is called before the first frame update
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
                puzzle = gameObject.AddComponent<QuizPuzzle>();
                break;
            case PUZZLE_TYPE.SOCCER:
                puzzle = gameObject.AddComponent<SoccerPuzzle>();
                break;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (activated && !puzzle.IsActivated())
        {
            puzzle.ActivatePuzzle(Random.Range(3,7));
        }

    }

    protected override bool CanProceed()
    {
        return puzzle.IsComplete();
    }
}
