using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HibouGerbille
{
    [System.Serializable]
    public class HowlBehavior : MonoBehaviour
    {

        [SerializeField] private AudioTabListener[] audioTab;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animHowl;
        [SerializeField] private bool launchMusic = true;
        [SerializeField] private bool increaseVolume = false;
        [SerializeField] private bool decreaseVolume = false;

        [SerializeField] private GameObject timelineDeath;
        [SerializeField] private GerbilBehaviour gerbilScript;
        [SerializeField] private Image detectionHowlCircle;
        [SerializeField] private GameObject exclamationFolder;
        [SerializeField] private Animator detectionAnim;
        private bool detected;
        private int countIdle = 0;
        private int randomClipItem;
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

            FillingDetectionCircle();
        }

        public void WaitBeforeChangeIdle()
        {
            //StopAllCoroutines();
            StartCoroutine(ChangeIdle());
        }

        public void WaitBeforeReturn()
        {
            //StopAllCoroutines();
            StartCoroutine(ReturnDefaultIdle());
        }

        public void WaitBeforeDefaultIdle()
        {
            StartCoroutine(IdleTurnToDefault());
        }


        public void ResetValues()
        {
            Debug.Log("ResetValues");
            animHowl.SetBool("Return", false);
            countIdle = 0;
            animHowl.SetInteger("CountIdle", countIdle);
        }

        IEnumerator ChangeIdle()
        {
            randomWait = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomWait);
            countIdle = Random.Range(1, 4);
            animHowl.SetInteger("CountIdle", countIdle);
        }

        IEnumerator ReturnDefaultIdle()
        {
            randomWait = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomWait);
            animHowl.SetBool("Return", true);
        }

        IEnumerator TurnBackCoroutine()
        {
            launchMusic = false;
            randomClipItem = Random.Range(0, audioTab.Length);
            audioSource.PlayOneShot(audioTab[randomClipItem].audioClip);
            yield return new WaitForSeconds(1f);
            increaseVolume = true;
            yield return new WaitForSeconds(audioTab[randomClipItem].clipDuration);
            decreaseVolume = true;
            StartCoroutine(IdleDefaultBeforeTurning());
        }

        IEnumerator IdleTurnToDefault()
        {
            countIdle = 0;
            yield return new WaitForSeconds(Random.Range(3, 5));
            animHowl.SetBool("Turning", false);
            launchMusic = true;
        }

        IEnumerator IdleDefaultBeforeTurning()
        {
            animHowl.SetBool("Return", true);
            float randomTurnWait = Random.Range(0.25f, 1.5f);
            Debug.Log(randomTurnWait);
            yield return new WaitForSeconds(randomTurnWait);
            animHowl.SetBool("Turning", true);
            detected = false;
        }

        void VolumeUp()
        {
            audioSource.volume += 0.01f;

            if(audioSource.volume == 1f)
            {
                increaseVolume = false;
                audioSource.volume = 1f;
            }

        }

        void VolumeDown()
        {
            audioSource.volume -= 0.01f;

            if (audioSource.volume == 0f)
            {
                decreaseVolume = false;
                audioSource.volume = 0f;
            }
        }

        void FillingDetectionCircle()
        {
            if(gerbilScript.detecting == true && !detected)
            {
                detectionHowlCircle.fillAmount += 0.01f;
                exclamationFolder.SetActive(true);

            }
            
            if(gerbilScript.detecting == false)
            {
                detectionHowlCircle.fillAmount -= 0.01f;
                exclamationFolder.SetActive(false);

            }

            if (detectionHowlCircle.fillAmount > 0.95f && !detected)
            {
                detectionHowlCircle.fillAmount = 0f;
                detected = true;
                detectionAnim.SetTrigger("Detected");
                StopAllCoroutines();
                StartCoroutine(IdleDefaultBeforeTurning());
            }
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
