using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using System.Linq;
using EntityCores;

namespace GameManagement.UIComps
{
    public class CutScene : MonoBehaviour, IDataPersistence
    {
        GameObject go;
        GameObject globalLight;
        Player player;
        Light2D light2D;
        PlayableDirector playable;
        bool alreadyActivated;


        private void Awake()
        {
            go = GameObject.Find("UIManager");
            globalLight = GameObject.Find("GlobalLight");
            player = FindObjectOfType<Player>(true);
            playable = GetComponent<PlayableDirector>();
            alreadyActivated = false;
            light2D = GetComponent<Light2D>();

        }

        private void Start()
        {
            if (!alreadyActivated)
            {
                LoadCutScene(alreadyActivated);
                alreadyActivated = true;
            }


        }

        public void LoadData(GameData data)
        {
            bool activated = data.rooms.Values.Any(i => i == 1);
            StartCoroutine(LoadCutScene(activated));
        }

        private IEnumerator LoadCutScene(bool activated)
        {
            if (!activated)
            {
                yield return new WaitForSeconds(1f);
                playable.Play();
                if (go != null)
                {
                    go.SetActive(false);
                }

                if (globalLight != null)
                {
                    globalLight.SetActive(false);
                }
            }
            else
            {
                if (go != null)
                {
                    go.SetActive(true);

                }

                if (globalLight != null)
                {
                    globalLight.SetActive(true);
                    var light = globalLight.GetComponent<Light2D>();
                    light.intensity = 0.4f;

                }
                this.gameObject.SetActive(false);
            }
        }

        public void SaveData(ref GameData data)
        {

        }
    }
}
