using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GRP18
{

    public class JaugeDecreasingDefeat : MonoBehaviour
    {
        [SerializeField] private Image jaugeUI;
        [SerializeField] private GerbilBehaviour FiScript;
        [SerializeField] private float speedMultiplierJauge;
        public bool launchDecrease;
        private bool emptyJauge;

        private void Update()
        {
            if(launchDecrease)
            {
                jaugeUI.fillAmount -= Time.deltaTime * speedMultiplierJauge;
            }

            if(jaugeUI.fillAmount == 0f && !emptyJauge)
            {
                emptyJauge = true;
                launchDecrease = false;
                FiScript.KillingFi();
            }
        }
    }
}
