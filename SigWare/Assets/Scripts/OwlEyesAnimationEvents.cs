using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRP18_TheGerbilAndTheOwl
{
    public class OwlEyesAnimationEvents : MonoBehaviour
    {
        [SerializeField] private OwlBehavior owlScript;


        public void StopWatchingFi()
        {
            owlScript.stopWatchingPath();
        }

    }
}
