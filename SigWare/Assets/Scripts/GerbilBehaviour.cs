using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HibouGerbille
{
    public class GerbilBehaviour : MonoBehaviour
    {

        [Range(0.1f, 1f)]
        [SerializeField] private float waitBeforeNoiseBarDecrease = 1f;
        [Range(0.0025f, 0.005f)]
        [SerializeField] private float decreaseAmount;
        [SerializeField] private Image noiseBar;

        [Range(0.1f,0.5f)]
        [SerializeField] private float movingTime = 1f;
        [SerializeField] private float returnMoveDuration;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AudioClip mouseSqueak;
        [SerializeField] private AudioSource sourceAudio;
        [SerializeField] private GameObject timelineDeath;

        private bool isMoving = false;
        private bool canMove = true;
        private bool noiseBarDecrease = true;
        private float percentMove;
        private Vector3 StartPos;
        private Vector3 EndPos;
        public Vector3 distanceMoving;
        [HideInInspector] public bool detecting;

        private void Start()
        {
            StartPos = transform.position;
            EndPos = transform.position;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isMoving && canMove)
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
            sourceAudio.PlayOneShot(mouseSqueak);
            isMoving = true;
            StartPos = transform.position;
            EndPos = StartPos + distanceMoving;
            JumpForward();
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
                Debug.Log("QuéPasa");
                timelineDeath.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Howl" && isMoving)
            {
                canMove = false;
                Debug.Log("QuéPasa");
                timelineDeath.SetActive(true);
            }
        }

        void NoiseBarGestion()
        {
            noiseBar.fillAmount -= decreaseAmount;
            if(noiseBar.fillAmount < .2f)
            {
                noiseBar.fillAmount = .2f;
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

            //if (detectionHowlCircle.fillAmount > 0.95f)
            //{
            //    detectionHowlCircle.fillAmount = 1f;

            //}
        }
    }
}
