using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores
{
    public class DoorAudioComp : EntityAudioComp
    {

        private AudioClip unlockWoodenClip;
        private AudioClip lockWoodenClip;
        private AudioClip unlockSteelClip;
        private AudioClip unlockTutorialClip;

        protected override void Awake()
        {
            base.Awake();
            unlockWoodenClip = Resources.Load("Sounds/Door/UnlockWooden") as AudioClip;
            lockWoodenClip = Resources.Load("Sounds/Door/LockWooden") as AudioClip;
            unlockSteelClip = Resources.Load("Sounds/Door/UnlockSteel") as AudioClip;
            unlockTutorialClip = Resources.Load("Sounds/Door/UnlockTut") as AudioClip;
            SetAudioProperties(0.05f, 20f, 0.6f);

        }

        public void LockAudio()
        {
            string doorname = this.name.Substring(0, 2);
            switch (doorname)
            {

                case ("D1"):
                case ("D3"):
                    audioSource.clip = unlockSteelClip;
                    audioSource.Play();
                    break;
                case ("D2"):
                    audioSource.clip = lockWoodenClip;
                    audioSource.Play();
                    break;
                case ("T1"):
                    audioSource.clip = unlockTutorialClip;
                    audioSource.Play();
                    break;
            }
        }

        public void UnlockAudio()
        {
            string doorname = this.name.Substring(0, 2);
            switch (doorname)
            {
                case ("D1"):
                case ("D3"):
                    audioSource.clip = unlockSteelClip;
                    audioSource.Play();
                    break;
                case ("D2"):
                    audioSource.clip = unlockWoodenClip;
                    audioSource.Play();
                    break;
                case ("T1"):
                    audioSource.clip = unlockTutorialClip;
                    audioSource.Play();
                    break;
            }
        }
    }
}
