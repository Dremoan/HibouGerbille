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
        [SerializeField] private Animator animOwl;
        [HideInInspector] public Animator OwlEyes;
        [SerializeField] private Animator animCam;
        [SerializeField] private GerbilBehaviour gerbilScript;
        [HideInInspector] public bool turned;

        [Header("=== Detection UI ===")]
        public GameObject detectionGameObject;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
 

        [Header("=== TutoSequence ===")]
        public bool tutoSequence = true;
        private bool gameEnded;
        private int tutoCount = 0;
        [SerializeField] private Animator indicationsTuto;

        void Update()
        {
            if (!tutoSequence && !gameEnded)
            {
            StartCoroutine(TurnBackCoroutine());
            }

        }


        IEnumerator TurnBackCoroutine()
        {
            tutoSequence = true;
            yield return new WaitForSeconds(Random.Range(minValue, maxValue));
            StartCoroutine(IdleDefaultBeforeTurning());
        }

        public IEnumerator TurnBackCoroutineTuto()
        {
            yield return new WaitForSeconds(Random.Range(minValue, maxValue));
            StartCoroutine(IdleDefaultBeforeTurning());
        }

        public void stopWatchingPath()
        {
            turned = false;
            gerbilScript.GetComponent<Animator>().SetInteger("PoseCount", 0);
            animOwl.SetBool("Turning", false);
            if(!gameEnded)
            {
            OwlEyes.SetInteger("RandomIdle", 0);
            gerbilScript.canMove = true;
            tutoSequence = false;
            if(tutoCount < 1)
                {
                tutoCount += 1;
                indicationsTuto.Play("Start");
                }
            }
        }

        public IEnumerator IdleDefaultBeforeTurning()
        {
            detectionGameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            detectionGameObject.SetActive(false);
            animOwl.SetBool("Turning", true);
            turned = true;
        }

        public void CamShake()
        {
            animCam.Play ("CamShake");
        }

        public void OwlEyesRandomize()
        {
            OwlEyes.SetInteger("RandomIdle", Random.Range(1, 3));
        }

        public void EndGame()
        {
            StopAllCoroutines();
            gameEnded = true;
        }
    }

 
}
