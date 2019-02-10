using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRP18
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private OwlBehavior owlScript;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GerbilBehaviour FiScript;

        public void StopWatchingFi()
        {
            owlScript.StopWatchingPath();
        }

        public void FiComingInTrigger()
        {
            StartCoroutine(FiScript.FiComingIn());
        }

        public void ExclamationFolder()
        {
            if(this.name == "Exclamation")
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
