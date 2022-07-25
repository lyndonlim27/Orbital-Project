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
    private Player _player;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        _player = FindObjectOfType<Player>();
        GameObject go = GameObject.Find("VideoScreen");
        if (go != null)
        {
            videoScreen = go.GetComponent<RawImage>();
        }
        SetUpUrls();
        _loadScene = FindObjectOfType<LoadScene>();
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
        urls["Level1StartScene"] = "https://lyndonlim27.github.io/OrbitalVideo/level1startscene.mp4";
    }

    public void PlayVideo(string currscene)
    {
        StartCoroutine(WaitForVideo(currscene));
    }

    private IEnumerator WaitForVideo(string currscene)
    {
        string urllink = "";
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audiosource in audioSources)
        {
            audiosource.mute = true;
        }
        videoPlayer.enabled = true;
        urls.TryGetValue(currscene, out urllink);
        videoPlayer.url = urllink;
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;
        while (_loadScene.GetComponentInChildren<Canvas>() != null)
            yield return null;
        videoScreen.enabled = true;
        videoPlayer.Play();
        _player.GetCurrentRoom().PauseGame();
        while (videoPlayer.isPlaying)
            yield return null;
        videoPlayer.enabled = false;
        videoScreen.enabled = false;
        foreach (AudioSource audiosource in audioSources)
        {
            audiosource.mute = false;
        }
        _player.GetCurrentRoom().ResumeGame();
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
