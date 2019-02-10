using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRP18
{

    public class PlayAudioClip : MonoBehaviour
    {
        [SerializeField] private AudioSource sourceSense;
        [SerializeField] private AudioClip senseClip;


        public void PlayClip()
        {
            sourceSense.PlayOneShot(senseClip);
        }

    }
}
