using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace GRP18_TheGerbilAndTheOwl
{
    [System.Serializable]
    public class GerbilBehaviour : MonoBehaviour
    {
        [Header("=== Noise Bar Gestion ===")]

        [Range(0.1f, 1f)]
        [SerializeField] private float waitBeforeNoiseBarDecrease = 1f;

        [Range(0.0025f, 0.005f)]
        [SerializeField] private float decreaseAmount;

        [Range(0.075f, 0.125f)]
        [SerializeField] private float fillBarAmount = .075f;
        [SerializeField] private Image noiseBar;
        [HideInInspector] public float noiseBarMultiplier = 1f;
        [HideInInspector] public bool detecting;
        public bool noiseBarDecrease = true;


        [Header("=== Gerbil Movement ===")]
        [Range(0.1f,0.5f)]
        [SerializeField] private float movingTime = 1f;
        [SerializeField] private Vector3 distanceMoving;
        private float returnMoveDuration;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AnimationCurve curveVerticalMove;
        [SerializeField] private AudioClip mouseSqueak;
        private bool isMoving = false;
        [HideInInspector] public bool canMove = true;
        private float percentMove;
        private float initialY;
        private Vector3 startPos;
        private Vector3 endPos;


        [Header("=== Gerbil Animations ===")]
        [SerializeField] private Animator gerbilAnimator;
        private int countPoses;



        [Header("=== Features ===")]
        [SerializeField] private AudioSource sourceAudio;
        [SerializeField] private ParticleSystem dustParticles;
        [SerializeField] private GameObject timelineDeath;
        [SerializeField] public OwlBehavior owlScript;

        [Header("=== TutoSequence ===")]
        [SerializeField] private int countTillTimeline;
        [SerializeField] private bool tutoSequence = true;
        [SerializeField] private CinemachineVirtualCamera camBeginning;
        [SerializeField] private GameObject timelineTutorial;
        [SerializeField] private float timelineDuration = 6f;



        private void Start()
        {
            startPos = transform.position;
            initialY = transform.position.y;
            endPos = transform.position;
        }
        void Update()
        {
            
            if (Input.GetMouseButtonUp(0) && !isMoving && canMove)
            {
                if (tutoSequence)
                {
                    countTillTimeline += 1;
                }
                startPos = endPos;
                StartCoroutine(BlockNoiseBarDecrease());
                StartCoroutine(EnableMoving());
            }

            if(isMoving)
            {
                JumpForward();
            }

            if(noiseBarDecrease)
            {
                NoiseBarGestion();
            }

            if (countTillTimeline == 5f)
            {
                EndTutorial();
            }
        }


        IEnumerator EnableMoving()
        {
            gerbilAnimator.SetTrigger("Jump");
            isMoving = true;
            startPos = transform.position;
            float newX = startPos.x + distanceMoving.x;
            float ratioedNewX = Mathf.InverseLerp(4f, 32f, newX);
            endPos = new Vector3(newX, initialY + curveVerticalMove.Evaluate(ratioedNewX), transform.position.z);
            yield return new WaitForSeconds(movingTime);
            isMoving = false;
            transform.position = endPos;
            returnMoveDuration = 0;
        }

        IEnumerator BlockNoiseBarDecrease()
        {
            noiseBar.fillAmount += fillBarAmount;
            noiseBarDecrease = false;
            yield return new WaitForSeconds(waitBeforeNoiseBarDecrease);
            noiseBarDecrease = true;
        }

        IEnumerator ShowingOwlPattern()
        {
            yield return new WaitForSeconds(timelineDuration -2f);
            owlScript.tutoSequence = false;
        }

        void JumpForward()
        {
            returnMoveDuration += Time.deltaTime;
            percentMove = returnMoveDuration / movingTime;
            float timePercent = curveSpeed.Evaluate(percentMove);
            transform.position = Vector3.Lerp(startPos, endPos, timePercent);

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<OwlBeam>() != null && isMoving)
            {
                canMove = false;
                timelineDeath.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<OwlBeam>() != null && isMoving)
            {
                canMove = false;
                timelineDeath.SetActive(true);
            }
        }

        void NoiseBarGestion()
        {
            noiseBar.fillAmount -= decreaseAmount * noiseBarMultiplier;
            if(noiseBar.fillAmount < .2f)
            {
                noiseBar.fillAmount = .2f;
                noiseBarMultiplier = 1f;
            }

            if(noiseBar.fillAmount > .95f)
            {
                StartCoroutine(owlScript.IdleDefaultBeforeTurning());
            }
        }

        public void JumpEffects()
        {
            dustParticles.Play();
            sourceAudio.PlayOneShot(mouseSqueak);
        }

        public void NoiseBarStop()
        {
            noiseBarDecrease = true;
            noiseBar.fillAmount = .2f;
        }

        private void EndTutorial()
        {
            countTillTimeline = 0;
            canMove = false;
            tutoSequence = false;
            camBeginning.m_Priority = 9;
            timelineTutorial.SetActive(true);
            StartCoroutine(ShowingOwlPattern());
        }
    }
}
