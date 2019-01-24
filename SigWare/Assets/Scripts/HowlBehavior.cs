using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HibouGerbille
{
    [System.Serializable]
    public class HowlBehavior : MonoBehaviour
    {
        [Header("=== References ===")]
        [SerializeField] private AudioTabListener[] audioTab;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animHowl;
        [SerializeField] private Animator animCam;
        [SerializeField] private Animator animScope;
        [SerializeField] private GameObject timelineDeath;
        [SerializeField] private GerbilBehaviour gerbilScript;
        private bool launchMusic = true;
        private int randomClipItem;
        private bool increaseVolume = false;
        private bool decreaseVolume = false;


        [Header("=== Detection UI ===")]
        [SerializeField] private Image detectionHowlCircle;
        [SerializeField] private GameObject exclamationFolderNoiseBar;
        [SerializeField] private GameObject exclamationFolderTurnHowl;
        [SerializeField] private Animator exclamationFolderAnim;
        [SerializeField] private Vector3 exclamationFolderTurnHowlStartPos;
        [HideInInspector] public bool detected;
        private float randomWait;

        void Update()
        {
            if(launchMusic)
            {
                StopAllCoroutines();
                StartCoroutine(TurnBackCoroutine());
            }

            if(increaseVolume)
            {
                VolumeUp();
            }

            if(decreaseVolume)
            {
                VolumeDown();
            }

            //if(!detected)
            //{
            //FillingDetectionCircle();
            //}

            //if(exclamationFolderTurnHowl.activeInHierarchy)
            //{
            //    detectionHowlCircle.fillAmount = 0f;
            //    exclamationFolderNoiseBar.SetActive(false);
            //}
            //else
            //{
            //ExclamationFolderActivation();
            //}
        }

        public void WaitBeforeDefaultIdle()
        {
            StartCoroutine(IdleTurnToDefault());
        }

        IEnumerator TurnBackCoroutine()
        {
            launchMusic = false;
            randomClipItem = Random.Range(0, audioTab.Length);
            audioSource.PlayOneShot(audioTab[randomClipItem].audioClip);
            yield return new WaitForSeconds(.5f);
            increaseVolume = true;
            yield return new WaitForSeconds(audioTab[randomClipItem].clipDuration);
            decreaseVolume = true;
            StartCoroutine(IdleDefaultBeforeTurning());
        }

        IEnumerator IdleTurnToDefault()
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            animHowl.SetBool("Turning", false);
            gerbilScript.NoiseBarStop();
            launchMusic = true;
        }

        public IEnumerator IdleDefaultBeforeTurning()
        {
            exclamationFolderTurnHowl.transform.localPosition = exclamationFolderTurnHowlStartPos;
            exclamationFolderTurnHowl.SetActive(true);
            float randomTurnWait = Random.Range(1f, 2f);
            yield return new WaitForSeconds(randomTurnWait);
            exclamationFolderAnim.SetTrigger("Detected");
            animHowl.SetBool("Turning", true);
            detected = false;
        }

        void VolumeUp()
        {
            audioSource.volume += 0.01f;

            if(audioSource.volume > 1f)
            {
                increaseVolume = false;
                audioSource.volume = 1f;
            }

        }

        void VolumeDown()
        {
            audioSource.volume -= 0.01f;

            if (audioSource.volume < 0f)
            {
                decreaseVolume = false;
                audioSource.volume = 0f;
            }
        }

        void FillingDetectionCircle()
        {
            if(gerbilScript.detecting == true)
            {
                detectionHowlCircle.fillAmount += 0.01f;
            }
            
            if(gerbilScript.detecting == false)
            {
                detectionHowlCircle.fillAmount -= 0.01f;

            }

            if (detectionHowlCircle.fillAmount > 0.9f && !detected)
            {
                detectionHowlCircle.fillAmount = 0f;
                gerbilScript.noiseBarMultiplier = 5f;
                detected = true;
                StopAllCoroutines();
                StartCoroutine(IdleDefaultBeforeTurning());
            }
        }

        void ExclamationFolderActivation()
        {
            if(detectionHowlCircle.fillAmount > 0.1f && !detected)
            {
                exclamationFolderNoiseBar.SetActive(true);
            }
            else
            {
                exclamationFolderNoiseBar.SetActive(false);
            }
        }

        public void CamShake()
        {
            animCam.Play ("CamShake");
        }

        public void ScopeIn()
        {
            animScope.Play("ScopeIn");
        }

        public void ScopeOut()
        {
            animScope.Play("ScopeOut");
        }
    }

    [System.Serializable]
    public class AudioTabListener
    {
        [SerializeField] private string clipName;
        [SerializeField] public AudioClip audioClip;
        [SerializeField] public float clipDuration;

    }
}
