using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Puzzle 
{

    /// <summary>
    /// Next wave of puzzle.
    /// </summary>
    public void Next();

    /// <summary>
    /// Fulfill condition.
    /// </summary>
    /// <param name="key"></param>
    public void Fulfill();

    /// <summary>
    /// Check if puzzle is complete.
    /// </summary>
    /// <returns></returns>
    public bool IsComplete();

    /// <summary>
    /// Activate the Puzzle.
    /// </summary>
    public void ActivatePuzzle(int seqs);

    public bool IsActivated();


}
