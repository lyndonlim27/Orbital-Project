using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private Dictionary<string,string> urls;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoScreen;
    private LoadScene _loadScene;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        GameObject go = GameObject.Find("VideoScreen");
        if (go != null)
        {
            videoScreen = go.GetComponent<RawImage>();
        }
        SetUpUrls();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void SetUpUrls()
    {
        urls = new Dictionary<string, string>();
        urls["TutorialLevelIntroScene"] = "https://lyndonlim27.github.io/OrbitalVideo/tutstartscenevid.mp4";
        urls["TutorialLevelEndScene"] = "https://lyndonlim27.github.io/OrbitalVideo/tutscenevid.mp4";
        urls["LastLevelStartScene"] = "https://lyndonlim27.github.io/OrbitalVideo/lastlevelstartscene.mp4";
        urls["LastLevelEndScene"] = "https://lyndonlim27.github.io/OrbitalVideo/endscenevid.mp4";
    }

    public void PlayVideo(string currscene)
    {
        Debug.Log("Entered play");
        StartCoroutine(WaitForVideo(currscene));
    }

    private IEnumerator WaitForVideo(string currscene)
    {
        string urllink = "";
        Debug.Log("entered");
        urls.TryGetValue(currscene, out urllink);
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoScreen.enabled = true;
        videoPlayer.Play();
        
        while (videoPlayer.isPlaying)
            yield return null;
        videoPlayer.enabled = false;
        videoScreen.enabled = false;
        //if (videoPlayer != null && videoScreen != null && urllink != "")
        //{
        //    //videoPlayer.url = urllink;
        //    //videoScreen.enabled = true;
        //    //Debug.Log("video clip length" + videoPlayer.clip.length);
        //    //Debug.Log("video length" + videoPlayer.clip.length);
        //    //Debug.LogError("Entered");
        //    videoPlayer.Play();
        //    Debug.Log(videoPlayer.clip);
        //yield return new WaitForSeconds((float)videoPlayer.clip.length);
        //if (videoPlayer.frame <= (long) videoPlayer.frameCount)
        //{
        //    yield return null;
        //}

        //while (videoPlayer.isPlaying)
        //{
        //    yield return null;
        //}

        //yield return new WaitForSeconds(videoPlayer.frameCount );



    }
}
