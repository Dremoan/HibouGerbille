using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HibouGerbille
{
    [System.Serializable]
    public class HowlBehavior : MonoBehaviour
    {

        [SerializeField] private AudioTabListener[] audioTab;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animHowl;
        [SerializeField] private bool launchMusic = true;

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
            audioSource.PlayOneShot(audioTab[randomClipItem].audioClip, 1);
            yield return new WaitForSeconds(audioTab[randomClipItem].clipDuration);
            audioSource.Stop();
            animHowl.SetBool("Return", true);
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            animHowl.SetBool("Turning", true);
        }

        IEnumerator IdleTurnToDefault()
        {
            countIdle = 0;
            yield return new WaitForSeconds(Random.Range(3, 5));
            animHowl.SetBool("Turning", false);
            launchMusic = true;
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
