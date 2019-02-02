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
        private bool canMove = true;
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
        [SerializeField] public HowlBehavior howlScript;


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
        }


        IEnumerator EnableMoving()
        {
            gerbilAnimator.SetTrigger("Jump");
            isMoving = true;
            startPos = transform.position;
            float newX = startPos.x + distanceMoving.x;
            float ratioedNewX = Mathf.InverseLerp(0f, 32f, newX);
            endPos = new Vector3(newX, initialY + curveVerticalMove.Evaluate(ratioedNewX), transform.position.z);
            yield return new WaitForSeconds(movingTime);
            isMoving = false;
            transform.position = endPos;
            returnMoveDuration = 0;
        }

        IEnumerator BlockNoiseBarDecrease()
        {
            noiseBar.fillAmount += .075f;
            noiseBarDecrease = false;
            yield return new WaitForSeconds(waitBeforeNoiseBarDecrease);
            noiseBarDecrease = true;
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
            if(other.gameObject.GetComponent<HowlBehavior>() != null && isMoving)
            {
                canMove = false;
                timelineDeath.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<HowlBehavior>() != null && isMoving)
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

            if(noiseBar.fillAmount > .8f)
            {
                noiseBarDecrease = false;
                StartCoroutine(howlScript.IdleDefaultBeforeTurning());
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


    }
}
