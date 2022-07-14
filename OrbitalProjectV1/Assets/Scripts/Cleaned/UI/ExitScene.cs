using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExitScene : MonoBehaviour
{

    [SerializeField] private string _nextSceneName;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoScreen;
    private LoadScene _loadScene;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _loadScene = FindObjectOfType<LoadScene>();
    }

    private void OnTriggerEnter2D()
    {
        StartCoroutine(WaitForCutScene());
    }

    private IEnumerator WaitForCutScene()
    {
        if (videoPlayer!= null && videoScreen != null)
        {
            videoScreen.enabled = true;
            videoPlayer.Play();
            Debug.Log(videoPlayer.clip.length);
            yield return new WaitForSeconds((float)videoPlayer.clip.length);
            videoPlayer.enabled = false;
            videoScreen.enabled = false;
            
        }

        _loadScene.NextScene(_nextSceneName);
    }
}
