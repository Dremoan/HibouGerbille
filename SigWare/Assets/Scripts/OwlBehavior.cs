using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GRP18_TheGerbilAndTheOwl
{
    [System.Serializable]
    public class OwlBehavior : MonoBehaviour
    {
        [Header("=== References ===")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Animator animOwl;
        [HideInInspector] public Animator OwlEyes;
        [SerializeField] private Animator animCam;
        [SerializeField] private GerbilBehaviour gerbilScript;
        private int randomClipItem;
        private bool increaseVolume = false;
        private bool decreaseVolume = false;


        [Header("=== Detection UI ===")]
        [SerializeField] private GameObject detectionGameObject;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
 

        [Header("=== TutoSequence ===")]
        public bool tutoSequence = true;

        void Update()
        {
            if (!tutoSequence)
            {
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

        }


        IEnumerator TurnBackCoroutine()
        {
            tutoSequence = true;
            yield return new WaitForSeconds(Random.Range(minValue, maxValue));
            StartCoroutine(IdleDefaultBeforeTurning());
        }

        public void stopWatchingPath()
        {
            animOwl.SetBool("Turning", false);
            OwlEyes.SetInteger("RandomIdle", 0);
            gerbilScript.canMove = true;
            tutoSequence = false;
        }

        public IEnumerator IdleDefaultBeforeTurning()
        {
            detectionGameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            detectionGameObject.SetActive(false);
            animOwl.SetBool("Turning", true);
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


        public void CamShake()
        {
            animCam.Play ("CamShake");
        }

        public void OwlEyesRandomize()
        {
            OwlEyes.SetInteger("RandomIdle", Random.Range(1, 3));
        }

    }

 
}
