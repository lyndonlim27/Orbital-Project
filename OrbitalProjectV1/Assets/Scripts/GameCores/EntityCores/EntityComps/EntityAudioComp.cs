using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores
{
    [RequireComponent(typeof(EntityBehaviour), typeof(AudioSource), typeof(AudioClip))]
    public class EntityAudioComp : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected bool inAudio;
        [SerializeField] protected AudioClip footStep;
        protected float defdist;
        protected float defpitch;
        protected float defvol;
        protected EntityBehaviour parent;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            inAudio = false;
            footStep = Resources.Load("Sounds/Player/FootStep") as AudioClip;
            defdist = audioSource.maxDistance;
            defpitch = audioSource.pitch;
            defvol = audioSource.volume;

        }

        protected virtual void OnEnable()
        {
            parent = GetComponent<EntityBehaviour>();
            inAudio = false;
        }

        //A general footstep audio for entities
        protected virtual IEnumerator FootStepAudio()
        {
            if (Valid())
            {
                inAudio = true;
                audioSource.clip = footStep;
                audioSource.Play();
                yield return new WaitForSeconds(footStep.length);
                inAudio = false;
            }
        }

        private bool Valid()
        {
            return !parent.GetData().floating && parent.GetData().moveable && !inAudio;
        }


        protected IEnumerator LoadSingleAudio(AudioClip audioClip)
        {
            audioSource.Stop();
            inAudio = true;
            float ogpitch = audioSource.pitch;
            audioSource.pitch = 1f;
            audioSource.clip = audioClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            inAudio = false;
            audioSource.pitch = ogpitch;

        }

        public void SetAudioProperties(float vol = 0, float maxDist = 0, float pitch = 0)
        {
            audioSource.maxDistance = maxDist == 0 ? defdist : maxDist;
            audioSource.pitch = pitch == 0 ? defpitch : pitch;
            audioSource.volume = vol == 0 ? defvol : vol;
        }

        public void PlaySingleAudio(AudioClip audioClip)
        {
            StartCoroutine(LoadSingleAudio(audioClip));
        }
    }
}
