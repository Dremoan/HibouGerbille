using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HibouGerbille
{
    public class GerbilBehaviour : MonoBehaviour
    {
        [Header("=== Noise Bar Gestion ===")]
        [Range(0.1f, 1f)]
        [SerializeField] private float waitBeforeNoiseBarDecrease = 1f;
        [Range(0.0025f, 0.005f)]
        [SerializeField] private float decreaseAmount;
        [SerializeField] private Image noiseBar;
        [HideInInspector] public float noiseBarMultiplier = 1f;
        [HideInInspector] public bool detecting;
        private bool noiseBarDecrease = true;

        [Header("=== Gerbil Movement ===")]
        [Range(0.1f,0.5f)]
        [SerializeField] private float movingTime = 1f;
        [SerializeField] private float returnMoveDuration;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AudioClip mouseSqueak;
        private bool isMoving = false;
        private bool canMove = true;
        private float percentMove;
        private Vector3 StartPos;
        private Vector3 EndPos;
        public Vector3 distanceMoving;


        [Header("=== Gerbil Animations ===")]
        [SerializeField] private Animator gerbilAnimator;
        private int countPoses;



        [Header("=== Features ===")]
        [SerializeField] private AudioSource sourceAudio;
        [SerializeField] private GameObject timelineDeath;
        [SerializeField] public HowlBehavior howlScript;


        private void Start()
        {
            StartPos = transform.position;
            EndPos = transform.position;
        }
        void Update()
        {
            if (Input.GetMouseButtonUp(0) && !isMoving && canMove)
            {
                StartPos = EndPos;
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


            GestionDetectionCircle();

        }


        IEnumerator EnableMoving()
        {
            gerbilAnimator.SetTrigger("Jump");
            sourceAudio.PlayOneShot(mouseSqueak);
            isMoving = true;
            StartPos = transform.position;
            EndPos = StartPos + distanceMoving;
            yield return new WaitForSeconds(movingTime);
            isMoving = false;
            transform.position = EndPos;
            returnMoveDuration = 0;
        }

        IEnumerator BlockNoiseBarDecrease()
        {
            noiseBar.fillAmount += .05f;
            noiseBarDecrease = false;
            yield return new WaitForSeconds(waitBeforeNoiseBarDecrease);
            noiseBarDecrease = true;
        }

        void JumpForward()
        {
            returnMoveDuration += Time.deltaTime;
            percentMove = returnMoveDuration / movingTime;
            float timePercent = curveSpeed.Evaluate(percentMove);
            transform.position = Vector3.Lerp(StartPos, EndPos, timePercent);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Howl" && isMoving)
            {
                canMove = false;
                timelineDeath.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Howl" && isMoving)
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
        }

        void GestionDetectionCircle()
        {
            if (noiseBar.fillAmount > 0.6f)
            {
                detecting = true;
            }
            else
            {
                detecting = false;
            }
        }
    }
}
