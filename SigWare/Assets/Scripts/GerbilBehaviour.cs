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

        [Header("=== WinSequence ===")]
        [SerializeField] private GameObject timelineWin;
        [SerializeField] private float delayCoroutineTimeline;
        private bool gameEnded;

        private void Start()
        {
            startPos = transform.position;
            initialY = transform.position.y;
            endPos = transform.position;
        }
        void Update()
        {
            if(transform.position.x > 26f && !gameEnded)
            {
                EndGame();
            }
            if (Input.GetMouseButtonUp(0) && !isMoving && canMove)
            {
                gerbilAnimator.SetInteger("PoseCount", 0);
                if (tutoSequence)
                {
                    countTillTimeline += 1;
                }
                startPos = endPos;
                StartCoroutine(EnableMoving());
            }

            if(isMoving)
            {
                JumpForward();
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
            float ratioedNewX = Mathf.InverseLerp(4f, 30f, newX);
            endPos = new Vector3(newX, initialY + curveVerticalMove.Evaluate(ratioedNewX), transform.position.z);
            yield return new WaitForSeconds(movingTime);
            isMoving = false;
            transform.position = endPos;
            returnMoveDuration = 0;
        }

        IEnumerator ShowingOwlPattern()
        {
            yield return new WaitForSeconds(timelineDuration -2f);
            StartCoroutine(owlScript.TurnBackCoroutineTuto());
        }

        IEnumerator WinTimelineCoroutine()
        {
            yield return new WaitForSeconds(delayCoroutineTimeline);
            StartCoroutine(EnableMoving());
            yield return new WaitForSeconds(.25f);
            StartCoroutine(EnableMoving());
            yield return new WaitForSeconds(.25f);
            StartCoroutine(EnableMoving());
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

        public void JumpEffects()
        {
            dustParticles.Play();
            sourceAudio.PlayOneShot(mouseSqueak);
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

        private void EndGame()
        {
            owlScript.detectionGameObject.SetActive(false);
            gameEnded = true;
            canMove = false;
            timelineWin.SetActive(true);
            owlScript.EndGame();
            owlScript.enabled = false;
            StartCoroutine(WinTimelineCoroutine());
        }
    }
}
