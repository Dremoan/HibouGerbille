using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HibouGerbille
{
    public class GerbilBehaviour : MonoBehaviour
    {

        public AnimationCurve curveSpeed;
        [SerializeField] private float returnMoveDuration;
        [SerializeField] private float speedMove = 10f;
        [SerializeField] private float movingTime = 1f;
        public bool isMoving = false;


        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                StopAllCoroutines();
                StartCoroutine(EnableMoving());
            }

            if (isMoving)
            {
                transform.Translate(transform.right * speedMove * Time.deltaTime);
            }
        }


        IEnumerator EnableMoving()
        {
            isMoving = true;
            yield return new WaitForSeconds(movingTime);
            isMoving = false;
        }
    }
}
