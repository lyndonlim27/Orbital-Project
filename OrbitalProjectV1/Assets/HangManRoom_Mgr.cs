using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangManRoom_Mgr : RoomManager
{
    private bool playing;

    protected override void Awake()
    {
        base.Awake();
        playing = false;
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        if (!playing && activated)
        {
            playing = true;
            StartCoroutine(ThankYou());
        }
        

        
    }

    private IEnumerator ThankYou()
    {
        textDescription.StartDescription("If you ever reached this part, I am surprised, not at your ability to complete the game, but at the fact that you didn't run into bugs!");
        yield return new WaitForSeconds(2f);
        textDescription.StartDescription("Thank you for playing the game, I hope you enjoyed it");
        yield return new WaitForSeconds(2f);
    }
}
