using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EntityDataMgt;

namespace EntityCores.Behavioural
{
    public class EnemyAudioComp : EntityAudioComp
    {

        private EnemyData enemyData;

        /**
         * Enemy AudioSources
         */

        //MaleAudios
        #region Variables
        #region AudioClips
        private List<AudioClip> malesTakeDamageAudio;
        private List<AudioClip> malesDoDamageAudio;

        private List<AudioClip> femalesTakeDamageAudio;
        private List<AudioClip> femalesDoDamageAudio;

        private List<AudioClip> nonhumanTakeDamageAudio;

        #endregion

        #endregion

        #region Monobehaviour
        protected override void Awake()
        {
            base.Awake();
            InitializeAudioLists();
        }

        private void Start()
        {
            InitializeMalesAudio();
            InitializeFemalesAudio();
            InitializeNonHumanAudio();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            enemyData = parent.GetData() as EnemyData;

        }
        #endregion

        #region Internal Methods
        private void InitializeAudioLists()
        {
            malesTakeDamageAudio = new List<AudioClip>();
            malesDoDamageAudio = new List<AudioClip>();
            femalesTakeDamageAudio = new List<AudioClip>();
            femalesDoDamageAudio = new List<AudioClip>();
            nonhumanTakeDamageAudio = new List<AudioClip>();
        }

        private void InitializeMalesAudio()
        {
            malesTakeDamageAudio.AddRange(Resources.LoadAll<AudioClip>("Sounds/Enemies/Male/TakeDamage"));
            malesDoDamageAudio.AddRange(Resources.LoadAll<AudioClip>("Sounds/Enemies/Male/DoDamage"));
        }

        private void InitializeFemalesAudio()
        {
            femalesTakeDamageAudio.AddRange(Resources.LoadAll<AudioClip>("Sounds/Enemies/Female/TakeDamage"));
            femalesDoDamageAudio.AddRange(Resources.LoadAll<AudioClip>("Sounds/Enemies/Female/DoDamage"));
        }

        private void InitializeNonHumanAudio()
        {
            //no audio yet.
        }

        /**
         * Instantiate damage audio.
         */
        public void InstantiateDamageAudio()
        {

            switch (enemyData.body)
            {
                case EnemyData.BODYTYPE.MEN:
                    int rand = Random.Range(0, malesDoDamageAudio.Count);
                    StartCoroutine(LoadAudio(rand, malesDoDamageAudio));
                    break;
                case EnemyData.BODYTYPE.WOMEN:
                    int rand2 = Random.Range(0, femalesDoDamageAudio.Count);
                    StartCoroutine(LoadAudio(rand2, femalesDoDamageAudio));
                    break;
                case EnemyData.BODYTYPE.MONSTER:
                    /*havent found audios yet*/
                    break;
            }

        }

        /**
         * Instantiate hurt audio.
         */
        public void InstantiateHurtAudio()
        {
            switch (enemyData.body)
            {
                case EnemyData.BODYTYPE.MEN:
                    int rand = Random.Range(0, malesTakeDamageAudio.Count);
                    StartCoroutine(LoadAudio(rand, malesTakeDamageAudio));
                    break;
                case EnemyData.BODYTYPE.WOMEN:
                    int rand2 = Random.Range(0, femalesTakeDamageAudio.Count);
                    StartCoroutine(LoadAudio(rand2, femalesTakeDamageAudio));
                    break;
                case EnemyData.BODYTYPE.MONSTER:
                    /*havent found audios yet*/
                    break;
            }

        }

        /**
         * Play audio.
         */
        public void InstantiateAttackAudio()
        {
            if (enemyData.attackAudios.Count > 0)
            {
                audioSource.pitch = 1f;
                audioSource.clip = enemyData.attackAudios[0];
                audioSource.Play();
            }

        }

        /**
         * Play audio.
         */
        public void InstantiateFootstepAudio()
        {
            StartCoroutine(FootStepAudio());

        }

        /**
         * Load Audio.
         */
        private IEnumerator LoadAudio(int rand, List<AudioClip> audioClips)
        {
            audioSource.Stop();
            inAudio = true;
            float ogpitch = audioSource.pitch;
            audioSource.pitch = 1f;
            audioSource.clip = audioClips[rand];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            inAudio = false;
            audioSource.pitch = ogpitch;

        }

        /**
         * FootStep Audio.
         */
        protected override IEnumerator FootStepAudio()
        {
            if (!inAudio)
            {
                audioSource.pitch = 0.6f;
            }

            return base.FootStepAudio();

        }



        #endregion
    }
}
