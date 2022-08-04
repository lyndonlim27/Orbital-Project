using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameManagement.Helpers
{
    public class ExitScene : MonoBehaviour
    {

        [SerializeField] private string _nextSceneName;
        //[SerializeField] private VideoPlayer videoPlayer;
        //[SerializeField] private RawImage videoScreen;
        [SerializeField] private string videoName;
        private LoadScene _loadScene;

        private void Awake()
        {
            // videoPlayer = GetComponent<VideoPlayer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _loadScene = FindObjectOfType<LoadScene>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(WaitForCutScene());
            }

        }

        private IEnumerator WaitForCutScene()
        {
            //if (videoPlayer!= null && videoScreen != null)
            //{
            //    videoScreen.enabled = true;
            //    videoPlayer.Play();
            //    Debug.Log(videoPlayer.clip.length);
            //    yield return new WaitForSeconds((float)videoPlayer.clip.length);
            //    videoPlayer.enabled = false;
            //    videoScreen.enabled = false;

            //}
            if (videoName != "" && videoName != null)
            {
                VideoManager videoManager = FindObjectOfType<VideoManager>();
                videoManager.PlayVideo(videoName);
                VideoPlayer vidplayer = videoManager.GetComponent<VideoPlayer>();
                while (vidplayer.enabled)
                    yield return null;
            }
            _loadScene.NextScene(_nextSceneName);
        }
    }
}
