using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace GRP18
{
    [System.Serializable]
    public class GerbilBehaviour : MonoBehaviour
    {
        [Header("=== Scripts ===")]
        [SerializeField] public OwlBehavior owlScript;
        [SerializeField] private GameManager manager;
        [SerializeField] private JaugeDecreasingDefeat jaugeScript;

        [Header("=== Numbers ===")]
        [Range(0.1f, 0.5f)]
        [SerializeField] private float movingTime = 1f;
        [SerializeField] private int countTillTimeline;
        [SerializeField] private float timelineDuration = 6f;
        [SerializeField] private float delayCoroutineTimeline;
        [SerializeField] private Vector3 distanceMoving;
        private Vector3 startPos;
        private Vector3 endPos;
        private float returnMoveDuration;
        private float percentMove;
        private float initialY;
        private int countPoses;

        [Header("=== Curves ===")]
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AnimationCurve curveVerticalMove;

        [Header("=== Bools ===")]
        [SerializeField] private bool tutoSequence = true;
        public bool canMove;
        private bool isMoving = false;
        private bool gameEnded;
        private bool detected;

        [Header("=== Audio ===")]
        [SerializeField] private AudioSource sourceAudio;
        [SerializeField] private AudioClip[] mouseSqueaks;
        [SerializeField] private AudioClip[] mouseSurprised;
        [SerializeField] private AudioClip laughClip;

        [Header("=== Visual ===")]
        [SerializeField] private Animator gerbilAnimator;
        [SerializeField] private Animator jumpUI;
        [SerializeField] private Animator mouseUI;
        [SerializeField] private ParticleSystem dustParticles;
        [SerializeField] private CinemachineVirtualCamera camBeginning;


        [Header("=== Game Objects ===")]
        [SerializeField] private GameObject jumpUIObj;
        [SerializeField] private GameObject timelineDeath;
        [SerializeField] private GameObject timelineWin;
        [SerializeField] private GameObject timelineTutorial;
        public GameObject exclamationSurprised;


        private void Start()
        {
            startPos = transform.position;
            initialY = transform.position.y;
            endPos = transform.position;
        }
        void Update()
        {
                EndGame();

            if (Input.GetMouseButtonUp(0) && !isMoving && canMove)
            {
                if (tutoSequence)
                {
                    countTillTimeline += 1;
                }
                JumpEnabled();
            }

            if(isMoving)
            {
                JumpForward();
            }

            if(!isMoving && owlScript.turned == true)
            {
                gerbilAnimator.SetInteger("PoseCount", Random.Range(1,2));
            }

            if (countTillTimeline == 5)
            {
                EndTutorial();
            }

            gerbilAnimator.SetBool("OwlTurned", owlScript.turned);

        }


        IEnumerator EnableMoving()
        {

            gerbilAnimator.SetTrigger("Jump");
            isMoving = true;
            startPos = transform.position;
            float newX = startPos.x + distanceMoving.x;
            float ratioedNewX = Mathf.InverseLerp(-2.4f, 28f, newX);
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

            for (int i = 0; i < 3; i++)
            {
            StartCoroutine(EnableMoving());
            yield return new WaitForSeconds(.25f);
            }
            gerbilAnimator.Play("FiWinAnim");


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
                KillingFi();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<OwlBeam>() != null && isMoving)
            {
                KillingFi();
            }
        }

        public void JumpDust()
        {
            dustParticles.Play();
        }

        public void JumpSound()
        {
            sourceAudio.PlayOneShot(mouseSqueaks[Random.Range(0, 3)]);
        }

        public void JumpSoundWin()
        {
            sourceAudio.volume = 1f;
            sourceAudio.PlayOneShot(laughClip);
        }

        private void EndTutorial()
        {
            jumpUI.SetTrigger("FadeOut");
            mouseUI.SetTrigger("FadeOut");
            countTillTimeline = 0;
            canMove = false;
            tutoSequence = false;
            camBeginning.m_Priority = 9;
            timelineTutorial.SetActive(true);
            StartCoroutine(ShowingOwlPattern());
        }

        private void EndGame()
        {
            if (transform.position.x > 26f && !gameEnded && !detected)
            {
            jaugeScript.launchDecrease = false;
            owlScript.detectionGameObject.SetActive(false);
            gameEnded = true;
            canMove = false;
            timelineWin.SetActive(true);
            owlScript.EndGameVictory();
            owlScript.enabled = false;
            manager.BlendMusics();
            StartCoroutine(WinTimelineCoroutine());
            }
        }

        public void ResetTrigger()
        {
            gerbilAnimator.ResetTrigger("Jump");
        }

        public void ResetPoseCount()
        {

            gerbilAnimator.SetInteger("PoseCount", 0);
        }

        public void VictoryAnimEvent()
        {
            manager.Victoire();
        }

        public IEnumerator FiComingIn()
        {
            for (int i = 0; i < 17; i++)
            {
                JumpEnabled();
                yield return new WaitForSeconds(0.2f);
            }
            canMove = true;
            jumpUIObj.SetActive(true);
        }

        private void JumpEnabled()
        {
            startPos = endPos;
            StartCoroutine(EnableMoving());
            gerbilAnimator.SetInteger("PoseCount", 0);
        }

        public void PoseSoundTrigger()
        {
            sourceAudio.PlayOneShot(mouseSurprised[Random.Range(0,1)]);
        }

        public void KillingFi()
        {
            jaugeScript.launchDecrease = false;
            manager.BlendMusics();
            detected = true;
            canMove = false;
            owlScript.EndGameDefeat();
            timelineDeath.SetActive(true);
        }
    }
}
