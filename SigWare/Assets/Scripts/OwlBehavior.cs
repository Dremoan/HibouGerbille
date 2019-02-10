using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace GRP18
{
    [System.Serializable]
    public class OwlBehavior : MonoBehaviour
    {
        [Header("=== Scripts ===")]
        [SerializeField] private JaugeDecreasingDefeat jaugeScript;
        [SerializeField] private GerbilBehaviour gerbilScript;
        [SerializeField] private GameManager manager;
        [SerializeField] private PostProcessVolume postPro;

        [Header("=== Audio ===")]
        [SerializeField] private AudioSource sourceAudio;
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioClip screech;
        [SerializeField] private AudioClip woosh1;
        [SerializeField] private AudioClip woosh2;

        [Header("=== Animation ===")]
        [SerializeField] private Animator indicationsTuto;
        [SerializeField] private Animator animOwl;
        [SerializeField] public Animator owlEyes;
        [SerializeField] private Animator animCam;

        [Header("=== Game Objects ===")]
        [SerializeField] private GameObject owlFaisceauObj;
        [SerializeField] private GameObject owlBeamObj;
        [SerializeField] private GameObject owlEyesObj;
        [SerializeField] private GameObject blueBackground;
        [SerializeField] private GameObject redBackground;
        [SerializeField] private GameObject dontMove;
        [SerializeField] private GameObject sneak;
        public GameObject detectionGameObject;
 
        [Header("=== Floats ===")]
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        private int tutoCount = 0;

        [Header("=== Bools ===")]
        public bool tutoSequence = true;
        public bool turned;
        private bool gameEnded;
        

        private void Start()
        {
            BlueBackground();
        }

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
            StartCoroutine(IdleDefaultBeforeTurningTuto());
        }

        public void StopWatchingPath()
        {
            if(gameEnded)
            {
                owlEyesObj.SetActive(false);
            }


            turned = false;
            animOwl.SetBool("Turning", false);


            if(!gameEnded)
            {
            owlEyes.SetInteger("RandomIdle", 0);
            gerbilScript.canMove = true;
            tutoSequence = false;


            if(tutoCount < 1)
                {
                tutoCount += 1;
                jaugeScript.launchDecrease = true;
                StartCoroutine(SneakUIAppear());
                musicAudioSource.Play();
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

        public IEnumerator IdleDefaultBeforeTurningTuto()
        {
            detectionGameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            detectionGameObject.SetActive(false);
            animOwl.SetBool("Turning", true);
            dontMove.SetActive(true);
            turned = true;
        }

        public void CamShake()
        {
            animCam.Play ("CamShake");
        }

        public void OwlEyesRandomize()
        {
            gerbilScript.exclamationSurprised.SetActive(true);
            owlEyes.SetInteger("RandomIdle", Random.Range(1, 3));
        }

        public void EndGameVictory()
        {
            StopAllCoroutines();
            gameEnded = true;
            StopWatchingPath();
        }

        public void EndGameDefeat()
        {
            StopAllCoroutines();
            gameEnded = true;

        }

        public void DisableOwlEyes()
        {
            owlEyesObj.SetActive(false);
        }

        public void Defeat()
        {
            manager.Defaite();
        }

        public void ScreechSound()
        {
            sourceAudio.pitch = 1f;
            sourceAudio.PlayOneShot(screech);
            StartCoroutine(WeightPostProIncrease());
        }

        public void WooshSound1()
        {
            sourceAudio.pitch = 2.5f;
            sourceAudio.PlayOneShot(woosh1);
        }

        public void WooshSound2()
        {
            sourceAudio.pitch = 2f;
            sourceAudio.PlayOneShot(woosh2);
        }


        IEnumerator WeightPostProIncrease()
        {
            for (int i = 0; i < 10; i++)
            {
                postPro.weight += 0.1f;
                yield return null;
            }
        }

        IEnumerator SneakUIAppear()
        {
            dontMove.GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(.25f);
            sneak.SetActive(true);
        }

        public void RedBackground()
        {
            blueBackground.SetActive(false);
            redBackground.SetActive(true);
        }

        public void BlueBackground()
        {
            redBackground.SetActive(false);
            blueBackground.SetActive(true);
        }
    }

 
}
